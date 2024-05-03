using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Archivebate_Scraper
{
    public partial class Form1 : Form
    {
        protected ChromeOptions _options = null;
        protected List<ChromeDriver> _drivers = new List<ChromeDriver>();
        CancellationTokenSource CTS;

        private delegate void TextSafeDelegate(string text);
        private void AddSuccessTextBoxSafe(string text)
        {
            if (textBoxSuccessURLs.InvokeRequired)
            {
                TextSafeDelegate del = new TextSafeDelegate(AddSuccessTextBoxSafe);
                textBoxSuccessURLs.Invoke(del, text);
            }
            else
            {
                textBoxSuccessURLs.Text += text;
            }
        }
        private void AddFailTextBoxSafe(string text)
        {
            if (textBoxFailURLs.InvokeRequired)
            {
                TextSafeDelegate del = new TextSafeDelegate(AddFailTextBoxSafe);
                textBoxFailURLs.Invoke(del, text);
            }
            else
            {
                textBoxFailURLs.Text += text;
            }
        }
        private void UpdateResultLabelSafe(string text)
        {
            if (labelResult.InvokeRequired)
            {
                TextSafeDelegate del = new TextSafeDelegate(UpdateResultLabelSafe);
                labelResult.Invoke(del, text);
            }
            else
            {
                labelResult.Text = text;
            }
        }

        public Form1()
        {
            InitializeComponent();
            CTS = new CancellationTokenSource();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {

            int successCount = 0;
            int failCount = 0;
            Task.Run(() =>
            {
                var updateProgress = new Progress<int>(value =>
                {
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() => progressBar1.Value = Math.Clamp(value, progressBar1.Minimum, progressBar1.Maximum)));
                        return;
                    }
                    progressBar1.Value = Math.Clamp(value, progressBar1.Minimum, progressBar1.Maximum);
                });
                var addProgress = new Progress<int>(value =>
                {
                    int result = progressBar1.Value + value;
                    result = Math.Clamp(result, progressBar1.Minimum, progressBar1.Maximum);
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() => progressBar1.Value = result));
                        return;
                    }
                    progressBar1.Value = result;
                });

                //드라이버 초기화
                ChromeDriverService _driverService = ChromeDriverService.CreateDefaultService();
                _driverService.HideCommandPromptWindow = true;
                _options = new ChromeOptions();
                _options.AddArgument("--disable-gpu");
                if (checkBoxHeadless.Checked)
                    _options.AddArgument("--headless");
                _options.AddArgument("--window-size=1280x720");
                _options.AddArgument("--disable-infobars");
                _options.AddArgument("--disable-extensions");
                //_options.AddArgument("--blink-settings=imagesEnabled=false");

                //캐싱용 변수
                List<string> videoPageUrls = new List<string>();
                List<string> videoFileUrls = new List<string>();
                string baseUrl = "https://www.archivebate.com/profile/";
                string pageOption = "?page=";
                string nextLine = ",\n";
                By XPathVerify = By.XPath("//*[@id=\"verify\"]");
                By XPathLastElement = By.XPath("//*[@id=\"app\"]/main/section/div/div/div/div/div[2]/div[4]");
                By XPathVideo = By.ClassName("video_item");
                string href = "href";

                int totalPageCount = int.Parse(textBoxPageTo.Text) - int.Parse(textBoxPageFrom.Text) + 1;
                int CounterCount = int.Parse(textBoxCounterCount.Text);
                int ScraperCount = int.Parse(textBoxScraperCount.Text);

                int quota = totalPageCount / CounterCount;

                List<Task> workers = new List<Task>();
                try
                {
                    for (int workerIndex = 0; workerIndex < CounterCount; workerIndex++)
                    {
                        int tempIndex = workerIndex;
                        workers.Add(Task.Run(() =>
                        {
                            ChromeDriver _driver = new ChromeDriver(_driverService, _options);
                            _drivers.Add(_driver);
                            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

                            int workersStartPage = tempIndex * quota + 1;
                            int workersEndPage = workersStartPage + quota - 1;
                            for (int page = workersStartPage; page <= workersEndPage; page++)
                            {
                                if (CTS.IsCancellationRequested)
                                    return;
                                _driver.Navigate().GoToUrl(baseUrl + textBoxID.Text + pageOption + page);
                                /*if (page == workersStartPage)
                                    _driver.FindElement(XPathVerify).Click();*/
                                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
                                var last = wait.Until(c => c.FindElement(XPathLastElement));
                                var elements = _driver.FindElements(XPathVideo);
                                for (int i = 1; i <= elements.Count; i++)
                                {
                                    if (CTS.IsCancellationRequested)
                                        return;
                                    videoPageUrls.Add(_driver.FindElement(By.XPath($"//*[@id=\"app\"]/main/section/div/div/div/div/div[2]/div[3]/section[{i}]/div[2]/a")).GetAttribute(href));
                                }
                                Task.Run(() => ((IProgress<int>)addProgress).Report((int)((float)page / workersEndPage * (50f / CounterCount))), CTS.Token);
                            }
                        }, CTS.Token));
                    }
                    Task.WaitAll(workers.ToArray());
                }
                finally
                {
                    workers.ForEach(worker => worker.Dispose());
                    workers.Clear();
                }
                Task.Run(() => ((IProgress<int>)updateProgress).Report(50), CTS.Token);

                quota = videoPageUrls.Count / ScraperCount;
                while (_drivers.Count < ScraperCount)
                {
                    ChromeDriver _driver = new ChromeDriver(_driverService, _options);
                    _drivers.Add(_driver);
                }
                try
                {
                    for (int workerIndex = 0; workerIndex < ScraperCount; workerIndex++)
                    {
                        int tempIndex = workerIndex;
                        workers.Add(Task.Run(async () =>
                        {
                            await Task.Run(() =>
                            {
                                WebDriverWait iframeWait = new WebDriverWait(_drivers[tempIndex], TimeSpan.FromSeconds(10));

                                int workersStartIndex = tempIndex * quota;
                                int workersEndIndex = Math.Clamp(workersStartIndex + quota, 0, videoPageUrls.Count);//-1

                                for (int i = workersStartIndex; i < workersEndIndex; i++)
                                {
                                    try
                                    {
                                        try
                                        {
                                            _drivers[tempIndex].Navigate().GoToUrl(videoPageUrls[i]);
                                        }
                                        catch (WebDriverException e)
                                        {
                                            Console.WriteLine(e);
                                            failCount++;
                                            AddFailTextBoxSafe(videoPageUrls[i] + nextLine);
                                        }
                                        IWebDriver videoFrame = null;

                                        try
                                        {
                                            videoFrame = iframeWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.TagName("IFRAME")));
                                        }
                                        catch (WebDriverTimeoutException)
                                        {
                                            failCount++;
                                            AddFailTextBoxSafe(videoPageUrls[i] + nextLine);
                                            continue;
                                        }

                                        IWebElement button = null;
                                        try
                                        {
                                            button = iframeWait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("vjs-big-play-button")));
                                        }
                                        catch (WebDriverTimeoutException)
                                        {
                                            failCount++;
                                            AddFailTextBoxSafe(videoPageUrls[i] + nextLine);
                                            //videoFileUrls.Add(videoPageUrls[i]);
                                            continue;
                                        }
                                        successCount++;
                                        _drivers[tempIndex].ExecuteScript("arguments[0].click();", button);

                                        var waitForElement = iframeWait.Until(ExpectedConditions.ElementExists(By.ClassName("vjs-tech")));

                                        AddSuccessTextBoxSafe(iframeWait.Until<string>(c =>
                                        {
                                            string result = waitForElement.GetAttribute("src");
                                            if (result != null && !result.Equals(""))
                                            {
                                                if (result.Contains("mxcontent") || result.Contains("xp-cdn.net") || result.Contains("video-delivery.net"))
                                                    return result;
                                                else
                                                    return videoPageUrls[i];
                                            }
                                            else
                                                return null;
                                        }) + nextLine);

                                        Task.Run(() => ((IProgress<int>)addProgress).Report((int)((float)i / quota * (50f / ScraperCount))), CTS.Token);
                                    }
                                    finally
                                    {
                                        UpdateResultLabelSafe($"Success: {successCount} / Fail: {failCount}");
                                    }
                                }
                            }, CTS.Token);
                            _drivers[tempIndex]?.Close();
                        }, CTS.Token));
                    }
                    Task.WaitAll(workers.ToArray());
                }
                finally
                {
                    workers.ForEach(worker => worker.Dispose());
                    workers.Clear();
                }
                MessageBox.Show("Done");
                Task.Run(() => ((IProgress<int>)updateProgress).Report(100), CTS.Token);
                UpdateResultLabelSafe($"Success: {successCount} / Fail: {failCount} / Total: {successCount + failCount}");
            }, CTS.Token);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CTS.Cancel();
            Task.Run(() =>
            {
                _drivers.ForEach(driver =>
                {
                    driver?.Quit();
                });
            });

        }
    }
}
