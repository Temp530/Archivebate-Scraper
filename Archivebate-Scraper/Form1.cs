using System.Diagnostics;
using System.Media;

namespace Archivebate_Scraper
{
    public partial class Form1 : Form
    {
        readonly ArchivebateClient _client = new();
        readonly CancellationTokenSource CTS = new();

        public Form1()
        {
            InitializeComponent();
        }

        // Queued, not blocking, so parallel requests never wait on the UI thread.
        private void OnUI(Action action)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(action);
        }

        // Only moves forward, since parallel updates can arrive out of order. Windows animates a growing bar
        // (it lags and looks unfinished at "Done") but draws a shrinking one at once, so step past and back.
        private void SetProgress(int value)
        {
            if (value <= progressBar1.Value)
                return;
            progressBar1.Maximum = 101;
            progressBar1.Value = value + 1;
            progressBar1.Value = value;
            progressBar1.Maximum = 100;
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            // Validate on the UI thread before starting: blank or 0 makes no page range or parallel count.
            string id = textBoxID.Text.Trim();
            int scraperCount = 0;
            if (id.Length == 0 || !int.TryParse(textBoxPageFrom.Text, out int pageFrom) || !int.TryParse(textBoxPageTo.Text, out int pageTo)
                || !int.TryParse(textBoxCounterCount.Text, out int counterCount)
                || pageFrom < 1 || pageTo < pageFrom || counterCount < 1
                || (!checkBoxOnlyPageURL.Checked && (!int.TryParse(textBoxScraperCount.Text, out scraperCount) || scraperCount < 1)))
            {
                MessageBox.Show("Enter the ID, and every number as 1 or more, with the first page not after the last page.");
                return;
            }

            buttonDownload.Enabled = false;
            progressBar1.Value = 0;
            try
            {
                await ScrapeAsync(id, pageFrom, pageTo, counterCount, checkBoxOnlyPageURL.Checked ? 0 : scraperCount, CTS.Token);
            }
            catch (OperationCanceledException)
            {
                return; // the form is closing
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Scraping stopped: " + ex.Message);
            }
            buttonDownload.Enabled = true;
        }

        // scraperCount 0 means "Page URL only": list the video pages without opening them.
        private async Task ScrapeAsync(string id, int pageFrom, int pageTo, int counterCount, int scraperCount, CancellationToken ct)
        {
            int success = 0, fail = 0, total = 0;
            void ShowResult() => labelResult.Text = $"Success: {success} / Fail: {fail} / Total: {total}";

            int pageCount = pageTo - pageFrom + 1, pagesDone = 0;
            labelResult.Text = $"Loading pages: 0 / {pageCount}";
            await _client.WarmUpAsync(ct);

            // Pages load in parallel but results keep page order.
            var pages = new List<string>[pageCount];
            await Parallel.ForEachAsync(Enumerable.Range(pageFrom, pageCount),
                new ParallelOptions { MaxDegreeOfParallelism = counterCount, CancellationToken = ct },
                async (page, token) =>
                {
                    try
                    {
                        pages[page - pageFrom] = await _client.GetVideoPageUrlsAsync(id, page, token);
                    }
                    catch (Exception ex) when (!token.IsCancellationRequested)
                    {
                        Debug.WriteLine(ex);
                        pages[page - pageFrom] = new();
                        OnUI(() => textBoxFailURLs.AppendText(ArchivebateClient.ProfilePageUrl(id, page) + Environment.NewLine));
                    }
                    int done = Interlocked.Increment(ref pagesDone);
                    OnUI(() =>
                    {
                        SetProgress(done * 50 / pageCount);
                        labelResult.Text = $"Loading pages: {done} / {pageCount}";
                    });
                });

            List<string> videoPageUrls = pages.SelectMany(p => p).Distinct().ToList();
            total = videoPageUrls.Count;
            SetProgress(50);
            ShowResult();

            if (scraperCount == 0)
            {
                success = total;
                textBoxSuccessURLs.AppendText(string.Concat(videoPageUrls.Select(url => url + Environment.NewLine)));
            }
            else
            {
                int videosDone = 0;
                await Parallel.ForEachAsync(videoPageUrls,
                    new ParallelOptions { MaxDegreeOfParallelism = scraperCount, CancellationToken = ct },
                    async (url, token) =>
                    {
                        string? src = null;
                        try
                        {
                            src = await _client.GetPlayerSourceAsync(url, token);
                        }
                        catch (Exception ex) when (!token.IsCancellationRequested)
                        {
                            Debug.WriteLine(ex);
                        }
                        // No player means the video was deleted; other players can't be resolved without a browser, so they are reported.
                        bool ok = src != null && ArchivebateClient.IsSupportedPlayer(src);
                        if (ok)
                            Interlocked.Increment(ref success);
                        else
                            Interlocked.Increment(ref fail);
                        string line = (ok || src == null ? url : $"{url} (unsupported player: {src})") + Environment.NewLine;
                        int done = Interlocked.Increment(ref videosDone);
                        OnUI(() =>
                        {
                            (ok ? textBoxSuccessURLs : textBoxFailURLs).AppendText(line);
                            SetProgress(50 + done * 50 / total);
                            ShowResult();
                        });
                    });
            }

            SetProgress(100);
            ShowResult();
            SystemSounds.Beep.Play();
            MessageBox.Show("Done");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CTS.Cancel();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label5.Visible = textBoxScraperCount.Visible = !checkBoxOnlyPageURL.Checked;
        }

        private void textBoxCounterCount_TextChanged(object sender, EventArgs e)
        {
            UpdatePageCounter();
        }
        // Boolean flag used to determine when a character other than a number is entered.
        private bool nonNumberEntered = false;

        // Handle the KeyDown event to determine the type of character entered into the control.
        private void textBoxOnlyNum_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Initialize the flag to false.
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }
            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }
        }
        private void textBoxOnlyNum_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
                MessageBox.Show("Only numeric input is accepted");
            }
        }

        private void textBoxPageFrom_TextChanged(object sender, EventArgs e)
        {
            UpdatePage();
        }

        private void textBoxPageTo_TextChanged(object sender, EventArgs e)
        {
            UpdatePage();
        }
        public void UpdatePage()
        {
            // Skip while a box is blank (user is mid-edit) instead of throwing FormatException.
            if (int.TryParse(textBoxPageFrom.Text, out int pageFrom) && int.TryParse(textBoxPageTo.Text, out int pageTo) && pageTo < pageFrom)
                textBoxPageTo.Text = textBoxPageFrom.Text;
            UpdatePageCounter();
        }
        public void UpdatePageCounter()
        {
            if (int.TryParse(textBoxCounterCount.Text, out int pageCounterCount) && int.TryParse(textBoxPageTo.Text, out int lastPage) && pageCounterCount > lastPage)
                textBoxCounterCount.Text = textBoxPageTo.Text;
        }
    }
}
