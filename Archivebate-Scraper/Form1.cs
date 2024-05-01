using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Archivebate_Scraper
{
    public partial class Form1 : Form
    {
        protected ChromeDriverService _driverService = null;
        protected ChromeOptions _options = null;
        protected ChromeDriver _driver = null;
        CancellationTokenSource CTS;

        private delegate void AddTextBoxSafeDelegate(string text);
        private void AddTextSafe(string text)
        {
            if (textBoxURLs.InvokeRequired)
            {
                AddTextBoxSafeDelegate del = new AddTextBoxSafeDelegate(AddTextSafe);
                textBoxURLs.Invoke(del, text);
            }
            else
            {
                textBoxURLs.Text += text;
            }
        }
        public Form1()
        {
            InitializeComponent();
            CTS = new CancellationTokenSource();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            var progress = new Progress<int>(value =>
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Value = value));
                    return;
                }
                progressBar1.Value = Math.Clamp(value, progressBar1.Minimum, progressBar1.Maximum);
            });

            _driverService = ChromeDriverService.CreateDefaultService();
            _driverService.HideCommandPromptWindow = true;

            _options = new ChromeOptions();
            _options.AddArgument("--disable-gpu");
            _options.AddArgument("--headless");
            _options.AddArgument("--window-size=1280x720");
            _options.AddArgument("--disable-infobars");
            _options.AddArgument("--disable-extensions");
            _options.AddArgument("--blink-settings=imagesEnabled=false");
            List<string> urls = new List<string>();
            string baseUrl = "https://www.archivebate.com/profile/";
            string pageOption = "?page=";
            string nextLine = "\n\r";
            By XPathVerify = By.XPath("//*[@id=\"verify\"]");
            By XPathLastElement = By.XPath("//*[@id=\"app\"]/main/section/div/div/div/div/div[2]/div[4]");
            By XPathVideo = By.ClassName("video_item");
            string href = "href";
            _driver = new ChromeDriver(_driverService, _options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Task.Run(() =>
            {
                var endPage = int.Parse(textBoxPageTo.Text);
                for (int page = int.Parse(textBoxPageFrom.Text); page <= endPage; page++)
                {
                    if (CTS.IsCancellationRequested)
                        return;
                    _driver.Navigate().GoToUrl(baseUrl + textBoxID.Text + pageOption + page);
                    if (page == 1)
                        _driver.FindElement(XPathVerify).Click();
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                    var last = wait.Until(c => c.FindElement(XPathLastElement));
                    var elements = _driver.FindElements(XPathVideo);
                    for (int i = 1; i <= elements.Count; i++)
                    {
                        if (CTS.IsCancellationRequested)
                            return;
                        urls.Add(_driver.FindElement(By.XPath($"//*[@id=\"app\"]/main/section/div/div/div/div/div[2]/div[3]/section[{i}]/div[2]/a")).GetAttribute(href) + nextLine);
                    }
                    Task.Run(() => ((IProgress<int>)progress).Report((int)(((float)page / endPage) * 100)), CTS.Token);
                }
                urls.ForEach(url =>
                {
                    AddTextSafe(url);
                });
                AddTextSafe("\n\r Done");
                MessageBox.Show("Done");
            }, CTS.Token);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CTS.Cancel();
            _driver?.Quit();
        }
    }
}
