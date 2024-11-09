using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Proxy;
using OpenQA.Selenium.Support.UI;
using System.Net;
using HtmlAgilityPack;

namespace Crawler.Infrastructure
{
    public class Core2
    {
        private static ChromeDriver Driver
        {
            get
            {
                var chromeOption = new ChromeOptions();
                //chromeOption.BinaryLocation = "D:\\TestProjects\\chrome-headless-shell-win64\\chrome-headless-shell.exe";
                //chromeOption.AddArguments("headless");
                chromeOption.AddArguments(new List<string>() { "no-sandbox", "headless", "disable-gpu" });
                chromeOption.BrowserVersion = "stable";
                chromeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.None;
                chromeOption.PlatformName = "windows";
                chromeOption.AddArguments("disable-infobars");
                chromeOption.AddExcludedArgument("enable-automation");
                chromeOption.AddAdditionalChromeOption("useAutomationExtension", false);
                chromeOption.Proxy = null;
                chromeOption.AddArguments("--disable-logging");
                chromeOption.AddArguments("--silent");
                chromeOption.AddArguments("--log-level=3");
                chromeOption.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
                chromeOption.AddArgument("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");

                var chromeService = ChromeDriverService.CreateDefaultService();
                chromeService.EnableVerboseLogging = false;
                chromeService.DisableBuildCheck = false;
                chromeService.SuppressInitialDiagnosticInformation = true;
                chromeService.HideCommandPromptWindow = true;
                return new ChromeDriver(chromeService, chromeOption);
            }
        }

        static async Task<List<string>> FilterProxiesAsync()
        {
            var proxies = new List<string>();
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://www.sslproxies.org/");
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            foreach (var row in htmlDocument.DocumentNode.SelectNodes("//table[@class='table']//tbody//tr"))
            {
                var ip = row.SelectSingleNode("td[1]").InnerText;
                var port = row.SelectSingleNode("td[2]").InnerText;
                proxies.Add($"{ip}:{port}");
            }

            return proxies;
        }

        public async Task Crawl(string url)
        {
            //List<string> allProxies = await FilterProxiesAsync();
            // Start the BrowserMob Proxy
            //var proxyServer = new BrowserMobProxyServer();
            //proxyServer.Start();
            //var seleniumProxy = new Proxy { HttpProxy = proxyServer.SeleniumProxy };


            // Add headers to the proxy
            //proxyServer.NewHar("tripadvisor");
            //proxyServer.AddHeader("Access-Control-Allow-Origin", "*");
            //proxyServer.AddHeader("Access-Control-Allow-Methods", "GET");
            //proxyServer.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            //proxyServer.AddHeader("accept", "*/*");
            //proxyServer.AddHeader("accept-encoding", "gzip, deflate");
            //proxyServer.AddHeader("accept-language", "en,mr;q=0.9");
            //proxyServer.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");

            // Initialize the Chrome driver
            var chromeOption = new ChromeOptions();
            chromeOption.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
            //chromeOption.AddArguments("headless");
            //chromeOption.AddArguments(new List<string>() { "no-sandbox", "headless", "disable-gpu" });
            //chromeOption.BrowserVersion = "stable";
            //chromeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.None;
            //chromeOption.PlatformName = "windows";
            //chromeOption.AddArguments("disable-infobars");
            //chromeOption.AddExcludedArgument("enable-automation");
            //chromeOption.AddAdditionalChromeOption("useAutomationExtension", false);
            //chromeOption.Proxy = null;
            //chromeOption.AddArguments("--disable-logging");
            //chromeOption.AddArguments("--silent");
            //chromeOption.AddArguments("--log-level=3");
            chromeOption.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            //chromeOption.AddArgument("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");

            var chromeService = ChromeDriverService.CreateDefaultService();
            //chromeService.EnableVerboseLogging = false;
            //chromeService.DisableBuildCheck = false;
            //chromeService.SuppressInitialDiagnosticInformation = true;
            //chromeService.HideCommandPromptWindow = true;
            var driver = new ChromeDriver(chromeService, chromeOption);
            //using var driver = Driver;

            // Navigate to the URL
            driver.Navigate().GoToUrl(url);

            // Wait for the page to load and the specific elements to be present
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(d => d.FindElement(By.CssSelector("div.all-results-section")));

            // Find all div elements with the class "XllAv H4 _a"
            var reviewElements = driver.FindElements(By.CssSelector("SVuzf e"));

            // Print the text of each review element
            foreach (var element in reviewElements)
            {
                Console.WriteLine(element.Text);
            }

            // Close the browser
            driver.Quit();
            //driver.Close();

        }
    }
}
