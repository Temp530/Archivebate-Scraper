using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;
using System.Media;
using Keys = System.Windows.Forms.Keys;

namespace Archivebate_Scraper
{
    public partial class Form1 : Form
    {
        protected ChromeOptions _options = null;
        protected List<ChromeDriver> _drivers = new List<ChromeDriver>();
        ChromeDriverService _driverService = ChromeDriverService.CreateDefaultService();
        CancellationTokenSource CTS;

        string baseUrl = "https://www.archivebate.com/profile/";
        string href = "href";
        string pageOption = "?page=";
        string nextLine = "\n";
        string emptyString = "";

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
                _driverService.HideCommandPromptWindow = true;
                _options = new ChromeOptions();
                if (checkBoxHeadless.Checked)
                {
                    _options.AddArgument("--disable-gpu");
                    _options.AddArgument("--headless");
                }
                _options.AddArgument("--window-size=1280x720");
                _options.AddArgument("--disable-infobars");
                _options.AddArgument("--disable-extensions");
                _options.AddArgument("--blink-settings=imagesEnabled=false");

                //캐싱용 변수
                List<string> videoPageUrls = new List<string>();
                List<string> videoFileUrls = new List<string>();
                By XPathVerify = By.XPath("//*[@id=\"verify\"]");
                By XPathLastElement = By.XPath("//*[@id=\"app\"]/main/section/div/div/div/div[2]/div[3]/section[20]");
                By VideoElement = By.ClassName("video_item");

                int destinationPage = int.Parse(textBoxPageTo.Text);
                int totalPageCount = destinationPage - int.Parse(textBoxPageFrom.Text) + 1;
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
                            int workersEndPage = Math.Clamp(workersStartPage + quota - 1, workersStartPage, destinationPage);
                            if (tempIndex.Equals(CounterCount - 1))
                                workersEndPage = destinationPage;
                            for (int page = workersStartPage; page <= workersEndPage; page++)
                            {
                                if (CTS.IsCancellationRequested)
                                    return;
                                _driver.Navigate().GoToUrl(baseUrl + textBoxID.Text + pageOption + page);
                                /*if (page == workersStartPage)
                                    _driver.FindElement(XPathVerify).Click();*/
                                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                                try
                                {
                                    wait.Until(ExpectedConditions.ElementIsVisible(XPathLastElement));
                                }
                                catch (WebDriverTimeoutException)
                                {
                                    //Last Page
                                }

                                var elements = _driver.FindElements(VideoElement);
                                for (int i = 1; i <= elements.Count; i++)
                                {
                                    if (CTS.IsCancellationRequested)
                                        return;
                                    lock (videoPageUrls)
                                        videoPageUrls.Add(_driver.FindElement(By.XPath($"//*[@id=\"app\"]/main/section/div/div/div/div[2]/section/div[3]/section[{i}]/div[2]/a")).GetAttribute(href));
                                }
                                int tempPage = page;
                                int tempworkersEndPage = workersEndPage;
                                Task.Run(() => ((IProgress<int>)addProgress).Report((int)((float)tempPage / tempworkersEndPage * (50f / CounterCount))), CTS.Token);
                            }
                        }, CTS.Token));
                    }
                    var workerArray = workers.ToArray();
                    Task.WaitAll(workerArray);
                }
                finally
                {
                    workers.ForEach(worker => worker.Dispose());
                    workers.Clear();
                    UpdateResultLabelSafe($"Success: {successCount} / Fail: {failCount} / Total: {videoPageUrls.Count}");
                }
                Task.Run(() => ((IProgress<int>)updateProgress).Report(50), CTS.Token);

                try
                {
                    if (checkBoxOnlyPageURL.Checked)
                    {
                        for (int i = 0; i < videoPageUrls.Count; i++)
                        {
                            AddSuccessTextBoxSafe(videoPageUrls[i] + nextLine);

                            Task.Run(() => ((IProgress<int>)updateProgress).Report(50 + (i / videoPageUrls.Count * 50)), CTS.Token);
                        }
                        Task.Run(() => ((IProgress<int>)updateProgress).Report(100), CTS.Token);
                        return;
                    }

                    quota = videoPageUrls.Count / ScraperCount;
                    if (quota < 1)
                    {
                        quota = 1;
                        ScraperCount = videoPageUrls.Count;
                    }

                    while (_drivers.Count < ScraperCount)
                    {
                        ChromeDriver _driver = new ChromeDriver(_driverService, _options);
                        _drivers.Add(_driver);
                    }

                    for (int workerIndex = 0; workerIndex < ScraperCount; workerIndex++)
                    {
                        int tempIndex = workerIndex;
                        workers.Add(
                            Task.Run(async () =>
                            {
                                await Task.Run(async () =>
                                {
                                    int workersStartIndex = tempIndex * quota;
                                    int workersEndIndex = Math.Clamp(workersStartIndex + quota, 0, videoPageUrls.Count);//-1
                                    if (tempIndex.Equals(ScraperCount - 1))
                                        workersEndIndex = videoPageUrls.Count;

                                    for (int i = workersStartIndex; i < workersEndIndex; i++)
                                    {
                                        WebDriverWait iframeWait = new WebDriverWait(_drivers[tempIndex], TimeSpan.FromSeconds(50));
                                        try
                                        {
                                            try
                                            {
                                                _drivers[tempIndex].Navigate().GoToUrl(videoPageUrls[i]);
                                            }
                                            catch (WebDriverException e)
                                            {
                                                Debug.WriteLine(e);
                                                lock (this)
                                                    failCount++;
                                                AddFailTextBoxSafe(videoPageUrls[i] + nextLine);
                                            }
                                            IWebElement button = null;
                                            try
                                            {
                                                string src = iframeWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"app\"]/main/section/div/div/div[2]/div/div[1]/div[1]/iframe"))).GetAttribute("src");
                                                string resultTemp = null;
                                                if (src.Contains("https://www.archivebate.com/embed") || (src.Contains("mixdrop.ag") && src.Contains("https://")))
                                                    resultTemp = videoPageUrls[i] + nextLine;
                                                if (resultTemp != null && !resultTemp.Equals(emptyString))
                                                {
                                                    lock (this)
                                                        successCount++;
                                                    AddSuccessTextBoxSafe(resultTemp);
                                                    continue;
                                                }
                                                iframeWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//*[@id=\"app\"]/main/section/div/div/div[2]/div/div[1]/div[1]/iframe")));
                                                button = iframeWait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("vjs-big-play-button")));
                                            }
                                            catch (WebDriverTimeoutException)
                                            {
                                                lock (this)
                                                    failCount++;
                                                AddFailTextBoxSafe(videoPageUrls[i] + nextLine);
                                                continue;
                                            }
                                            _drivers[tempIndex].ExecuteScript("arguments[0].click();", button);

                                            var waitForElement = iframeWait.Until(ExpectedConditions.ElementExists(By.ClassName("vjs-tech")));

                                            lock (this)
                                                successCount++;
                                            AddSuccessTextBoxSafe(iframeWait.Until<string>(c =>
                                            {
                                                string result = waitForElement.GetAttribute("src");
                                                if (result != null && !result.Equals(emptyString))
                                                {
                                                    if (result.Contains("video-delivery.net"))
                                                        return videoPageUrls[i];
                                                    else
                                                        return result;
                                                }
                                                else
                                                    return null;
                                            }) + nextLine);
                                        }
                                        finally
                                        {
                                            Task.Run(() => ((IProgress<int>)updateProgress).Report(50 + (int)((float)(successCount + failCount) / videoPageUrls.Count * 50f)), CTS.Token);
                                            UpdateResultLabelSafe($"Success: {successCount} / Fail: {failCount} / Total: {videoPageUrls.Count}");
                                        }
                                    }
                                }, CTS.Token);
                                _drivers[tempIndex]?.Close();
                            }, CTS.Token)
                        );
                    }
                    var workerArray = workers.ToArray();
                    Task.WaitAll(workerArray);
                }
                finally
                {
                    workers.ForEach(worker => worker.Dispose());
                    workers.Clear();
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Done");
                    Task.Run(() => ((IProgress<int>)updateProgress).Report(100), CTS.Token);
                    UpdateResultLabelSafe($"Success: {successCount} / Fail: {failCount} / Total: {videoPageUrls.Count}");
                }
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
                    foreach (Process P in Process.GetProcessesByName("chromedriver"))
                        P.Kill();
                });
                _driverService?.Dispose();
            });

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label5.Visible = textBoxScraperCount.Visible = !checkBoxOnlyPageURL.Checked;
        }

        private void textBoxCounterCount_TextChanged(object sender, EventArgs e)
        {
            int pageCounterCount = int.Parse(textBoxCounterCount.Text);
            int lastPage = int.Parse(textBoxPageTo.Text);
            if (pageCounterCount > lastPage)
                textBoxCounterCount.Text = textBoxPageTo.Text;
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
    }
}
