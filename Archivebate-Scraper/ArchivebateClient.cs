using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Archivebate_Scraper
{
    // Scrapes over plain HTTP: the profile video list is rendered by a Livewire v2 call, so no browser is needed.
    public sealed partial class ArchivebateClient : IDisposable
    {
        const string BaseUrl = "https://www.archivebate.com";
        const string VideosComponent = "profile.model-videos";
        const string VideosMethod = "load_profile_videos";
        const int MaxAttempts = 3;

        readonly HttpClient _http = new(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All,
            CookieContainer = new CookieContainer(),
        })
        { Timeout = TimeSpan.FromSeconds(30) };

        public ArchivebateClient()
        {
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0 Safari/537.36");
        }

        public static string ProfilePageUrl(string id, int page) => $"{BaseUrl}/profile/{Uri.EscapeDataString(id)}?page={page}";

        // Run once before going parallel so every request shares one Laravel session; otherwise CSRF tokens clash (HTTP 419).
        public Task WarmUpAsync(CancellationToken ct) => RetryAsync(() => _http.GetStringAsync(BaseUrl + "/", ct), ct);

        // Video page (/watch/...) URLs listed on one profile page, in page order.
        public Task<List<string>> GetVideoPageUrlsAsync(string id, int page, CancellationToken ct) => RetryAsync(async () =>
        {
            string pageUrl = ProfilePageUrl(id, page);
            string html = await _http.GetStringAsync(pageUrl, ct);
            string token = CsrfRegex().Match(html) is { Success: true } m ? m.Groups[1].Value
                : throw new InvalidDataException("CSRF token not found: " + pageUrl);
            JsonNode component = FindComponent(html, VideosComponent)
                ?? throw new InvalidDataException("Video list not found (wrong ID?): " + pageUrl);

            // fingerprint and serverMemo go back untouched: the server verifies their checksum.
            var body = new JsonObject
            {
                ["fingerprint"] = component["fingerprint"]!.DeepClone(),
                ["serverMemo"] = component["serverMemo"]!.DeepClone(),
                ["updates"] = new JsonArray(new JsonObject
                {
                    ["type"] = "callMethod",
                    ["payload"] = new JsonObject { ["id"] = "load", ["method"] = VideosMethod, ["params"] = new JsonArray() },
                }),
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/livewire/message/{VideosComponent}")
            {
                Content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json"),
            };
            request.Headers.Add("X-Livewire", "true");
            request.Headers.Add("X-CSRF-TOKEN", token);
            request.Headers.Referrer = new Uri(pageUrl);

            using var response = await _http.SendAsync(request, ct);
            response.EnsureSuccessStatusCode(); // a 419 lands here and the retry fetches a fresh token
            JsonNode? reply = JsonNode.Parse(await response.Content.ReadAsStringAsync(ct));
            return ParseVideoPageUrls(reply?["effects"]?["html"]?.GetValue<string>() ?? "");
        }, ct);

        // PlayerSrc is null when there is no player (e.g. "This video has been deleted.").
        public record VideoPage(string? PlayerSrc, DateOnly? Uploaded);

        public Task<VideoPage> GetVideoPageAsync(string videoPageUrl, CancellationToken ct) => RetryAsync(async () =>
        {
            string html = await _http.GetStringAsync(videoPageUrl, ct);
            return new VideoPage(
                IframeRegex().Match(html) is { Success: true } m ? WebUtility.HtmlDecode(m.Groups[1].Value) : null,
                ParseUploadDate(html));
        }, ct);

        // File size in bytes from the mixdrop file page (/f/ shows "name.mp4 385.16 MB"); null for other players.
        public async Task<long?> GetFileSizeAsync(string playerSrc, CancellationToken ct)
        {
            if (!playerSrc.Contains("mixdrop.") || !playerSrc.Contains("/e/"))
                return null;
            string html = await RetryAsync(() => _http.GetStringAsync(playerSrc.Replace("/e/", "/f/"), ct), ct);
            return ParseFileSize(html);
        }

        // Binary units, so a parsed "385.16 MB" formats back to the same text.
        public static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            double value = bytes;
            int unit = 0;
            for (; value >= 1024 && unit < units.Length - 1; unit++)
                value /= 1024;
            return $"{value.ToString(unit == 0 ? "0" : "0.00", CultureInfo.InvariantCulture)} {units[unit]}";
        }

        static long? ParseFileSize(string html)
        {
            Match m = FileSizeRegex().Match(html);
            if (!m.Success || !double.TryParse(m.Groups[1].Value.Replace(",", ""), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return null;
            int power = "BKMGT".IndexOf(char.ToUpperInvariant(m.Groups[2].Value[0]));
            return (long)Math.Round(value * Math.Pow(1024, power));
        }

        // The site prints the upload day as dd.MM.yy, e.g. "30.09.26 · Chaturbate".
        static DateOnly? ParseUploadDate(string html) =>
            UploadDateRegex().Match(html) is { Success: true } m
                && DateOnly.TryParseExact(m.Groups[1].Value, "dd.MM.yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date)
                ? date : null;

        // Players a downloader can take the video page URL for (the old Selenium code's success rule).
        public static bool IsSupportedPlayer(string src) =>
            src.Contains("archivebate.com/embed") || (src.StartsWith("https://") && src.Contains("mixdrop."));

        static List<string> ParseVideoPageUrls(string html) =>
            WatchLinkRegex().Matches(html).Select(m => m.Groups[1].Value).Distinct().ToList();

        static JsonNode? FindComponent(string html, string name) =>
            InitialDataRegex().Matches(html)
                .Select(m => JsonNode.Parse(WebUtility.HtmlDecode(m.Groups[1].Value)))
                .FirstOrDefault(n => n?["fingerprint"]?["name"]?.GetValue<string>() == name);

        // Retries timeouts and HTTP errors (5xx, 429, 419, dropped connections) with a short backoff; 404 (wrong ID) fails at once.
        static async Task<T> RetryAsync<T>(Func<Task<T>> op, CancellationToken ct)
        {
            for (int attempt = 1; ; attempt++)
            {
                try
                {
                    return await op();
                }
                catch (Exception e) when (attempt < MaxAttempts && !ct.IsCancellationRequested
                    && e is HttpRequestException { StatusCode: not HttpStatusCode.NotFound } or TaskCanceledException)
                {
                    await Task.Delay(TimeSpan.FromSeconds(attempt), ct);
                }
            }
        }

        public void Dispose() => _http.Dispose();

        [GeneratedRegex("<meta name=\"csrf-token\" content=\"([^\"]+)\"")]
        private static partial Regex CsrfRegex();

        [GeneratedRegex("wire:initial-data=\"([^\"]*)\"")]
        private static partial Regex InitialDataRegex();

        [GeneratedRegex("href=\"(https://(?:www\\.)?archivebate\\.com/watch/\\d+)\"")]
        private static partial Regex WatchLinkRegex();

        [GeneratedRegex("<iframe[^>]*\\ssrc=\"([^\"]+)\"")]
        private static partial Regex IframeRegex();

        [GeneratedRegex("<div class=\"info\">\\s*<p>\\s*(\\d{2}\\.\\d{2}\\.\\d{2})")]
        private static partial Regex UploadDateRegex();

        [GeneratedRegex("</b>\\s*([\\d.,]+)\\s*([KMGT]?B)\\s*<", RegexOptions.IgnoreCase)]
        private static partial Regex FileSizeRegex();

        // Parsers checked against canned markup shaped like the live site (Debug builds only).
        [Conditional("DEBUG")]
        public static void SelfCheck()
        {
            const string page = "<meta name=\"csrf-token\" content=\"tok\">"
                + "<div wire:initial-data=\"{&quot;fingerprint&quot;:{&quot;name&quot;:&quot;follow-streamer&quot;}}\"></div>"
                + "<div wire:initial-data=\"{&quot;fingerprint&quot;:{&quot;name&quot;:&quot;profile.model-videos&quot;},&quot;serverMemo&quot;:{&quot;checksum&quot;:&quot;c&quot;}}\" wire:init=\"load_profile_videos\"></div>";
            Debug.Assert(CsrfRegex().Match(page).Groups[1].Value == "tok");
            Debug.Assert(FindComponent(page, VideosComponent)?["serverMemo"]?["checksum"]?.GetValue<string>() == "c");

            var urls = ParseVideoPageUrls("<a href=\"https://www.archivebate.com/watch/2\"></a><a href=\"https://www.archivebate.com/watch/1\"></a>"
                + "<a href=\"https://www.archivebate.com/watch/2\"></a><a href=\"https://www.archivebate.com/profile/x\"></a>");
            Debug.Assert(urls.SequenceEqual(new[] { "https://www.archivebate.com/watch/2", "https://www.archivebate.com/watch/1" }));

            Debug.Assert(IframeRegex().Match("<div class=\"player\"> <iframe src=\"https://mixdrop.ag/e/abc\" scrolling=\"no\">").Groups[1].Value == "https://mixdrop.ag/e/abc");
            Debug.Assert(ParseUploadDate("<div class=\"info\">\n <p>\n 30.09.26 &middot; Chaturbate <br>") == new DateOnly(2026, 9, 30));
            long? size = ParseFileSize("<b title=\"a.mp4\">a.mp4</b> 385.16 MB </div>");
            Debug.Assert(size is not null && FormatSize(size.Value) == "385.16 MB" && FormatSize(3L << 30) == "3.00 GB");
            Debug.Assert(IsSupportedPlayer("https://mixdrop.ag/e/abc") && IsSupportedPlayer("https://www.archivebate.com/embed/1") && !IsSupportedPlayer("https://other.host/e/1"));
        }
    }
}
