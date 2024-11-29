using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Proxy;
using OpenQA.Selenium.Support.UI;
using System.Net;
using HtmlAgilityPack;
using System.Runtime;
using System.Reflection;
using System.Xml.Linq;

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
                //chromeOption.AddArgument("--headless");

                //chromeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
                chromeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Eager;

                //chromeOption.AddArguments("--blink-settings=imagesEnabled=false");
                //chromeOption.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

                chromeOption.PlatformName = "windows";
                chromeOption.BrowserVersion = "stable";
                chromeOption.AddArguments("start-maximized"); // open Browser in maximized mode
                chromeOption.AddArguments("--disable-extensions"); // disabling extensions
                chromeOption.AddArguments("--disable-gpu"); // applicable to windows os only
                chromeOption.AddArguments("--disable-dev-shm-usage"); // overcome limited resource problems
                chromeOption.AddArguments("--no-sandbox"); // Bypass OS security model
                //chromeOption.AddArguments(new List<string>() { "no-sandbox", "headless", "disable-gpu" });
                chromeOption.AddArguments("disable-infobars");
                chromeOption.AddExcludedArgument("enable-automation");
                chromeOption.AddAdditionalChromeOption("useAutomationExtension", false);
                chromeOption.Proxy = null;
                chromeOption.AddArguments("--disable-logging");
                chromeOption.AddArguments("--silent");
                chromeOption.AddArguments("--log-level=3");
                chromeOption.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
                //chromeOption.AddArgument("user-data-dir=C:\\Users\\amine\\AppData\\Local\\Google\\Chrome\\User Data");

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

        public async Task<Dictionary<string, string>> Crawl1(string url)
        {
            Dictionary<string, string> result = new();
            var driver = Driver;

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            string titles = string.Empty;
            int index = 0;
            int repeatCount = 30;
        here:
            do
            {
                if (index == 0)
                {
                    driver.Navigate().GoToUrl(url);
                }
                try
                {
                    if (index > 0)
                    {
                        await driver.Navigate().RefreshAsync();
                    }

                    await Task.Delay(500);

                    IWebElement titleDivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("div[class*='Titles_titles']")));
                    titles = titleDivElement.Text;
                    if (string.IsNullOrEmpty(titles))
                    {
                        index++;
                        //Console.WriteLine($"Not Found {url} ({index})");
                    }
                    else
                    {
                        Console.WriteLine($"Found {url} ({index})");
                        index = repeatCount;
                    }
                }
                catch
                {
                    try
                    {
                        IWebElement unavailableDivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("div[class*='BuyBox_unavailable-buy-box']")));
                        index = repeatCount;
                        titles = "کالا ناموجود\r\nUnavailable Product";
                        break;
                    }
                    catch { }

                    try
                    {
                        IWebElement error404DivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("button[class*='error-container_error']")));
                        index = repeatCount;
                        titles = "صفحه مورد نظر یافت نشد 404\r\nPage not found 404";
                        break;
                    }
                    catch { }
                    index++;
                }
            }
            while (index < repeatCount);


            driver.Quit();
            var listTitle = titles.Trim().Split("\r\n");


            Dictionary<string, string> name = new Dictionary<string, string>();
            if (listTitle != null)
            {
                if (listTitle.Length > 0 && !String.IsNullOrEmpty(listTitle[0]))
                {
                    name.Add("fa", listTitle[0]);
                }
                else
                {
                    name.Add("fa", "");
                }

                if (listTitle.Length > 1 && !String.IsNullOrEmpty(listTitle[1]))
                {
                    name.Add("en", listTitle[1]);
                }
                else
                {
                    name.Add("en", "");
                }
                if (name["en"] != "")
                {
                    result.Add($"{url} ({index})", name["en"]);
                }
                else
                {
                    result.Add($"{url} ({index})", name["fa"]);
                }
            }


            return result;
        }

        public async Task<Dictionary<string, string>> Crawl2(string url)
        {
            Dictionary<string, string> result = new();
            using var driver = Driver;

            string titles = string.Empty;
            int index = 0;
        here:

            try
            {
                if (index == 0)
                {
                    driver.Navigate().GoToUrl(url);
                }
                if (index > 0)
                {
                    await driver.Navigate().RefreshAsync();
                }
                await Task.Delay((int)(0.5 * 1000));
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                IWebElement titleDivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("div[class*='Titles_titles']")));
                titles = titleDivElement.Text;
                if (string.IsNullOrEmpty(titles))
                {
                    index++;
                    Console.WriteLine($"Not Found {url} ({index})");
                    goto here;
                }
                else
                {
                    Console.WriteLine($"Found {url} ({index})");
                    //index = repeatCount;
                }
            }
            catch
            {
                index++;
                goto here;
            }

            driver.Quit();
            var listTitle = titles.Trim().Split("\r\n");


            Dictionary<string, string> name = new Dictionary<string, string>();
            if (listTitle != null)
            {
                if (listTitle.Length > 0 && !String.IsNullOrEmpty(listTitle[0]))
                {
                    name.Add("fa", listTitle[0]);
                }
                else
                {
                    name.Add("fa", "");
                }

                if (listTitle.Length > 1 && !String.IsNullOrEmpty(listTitle[1]))
                {
                    name.Add("en", listTitle[1]);
                }
                else
                {
                    name.Add("en", "");
                }
                if (name["en"] != "")
                {
                    result.Add($"{url} ({index})", name["en"]);
                }
                else
                {
                    result.Add($"{url} ({index})", name["fa"]);
                }
            }


            return result;
        }

        public async Task<ChromeDriver?> InitializeCrawler(string url, int repeatCount = 30, decimal delay = 0.5m)
        {
            bool find = false;
            try
            {
                ChromeDriver driver = Driver;
            }
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            string titles = string.Empty;
            int index = 0;
            do
            {
                try
                {
                    if (index == 0)
                    {
                        driver.Navigate().GoToUrl(url);
                    }
                    if (index > 0)
                    {
                        await driver.Navigate().RefreshAsync();
                    }


                    await Task.Delay((int)(delay * 1000));

                    IWebElement titleDivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("div[class*='Titles_titles']")));
                    titles = titleDivElement.Text;
                    if (string.IsNullOrEmpty(titles))
                    {
                        index++;
                    }
                    else
                    {
                        find = true;
                        index = repeatCount;
                    }
                }
                catch
                {
                    index++;
                    try
                    {
                        IWebElement unavailableDivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("div[class*='BuyBox_unavailable-buy-box']")));
                        find = false;
                        index = repeatCount;
                    }
                    catch { }

                    try
                    {
                        IWebElement error404DivElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("button[class*='error-container_error']")));
                        find = false;
                        index = repeatCount;
                    }
                    catch { }
                }
            }
            while (index < repeatCount);

            if (find)
            {
                return driver;
            }

            driver.Quit();
            return null;
        }

        public async Task<Dictionary<string, string>> Crawl3(string url)
        {
            var driver = Driver;
            Dictionary<string, string> result = await Crawl4(Driver, url);
            driver.Quit();

            return result;
        }
        public async Task<Dictionary<string, string>> Crawl4(ChromeDriver? driver, string url)
        {
            Dictionary<string, string> result = new();

            driver = driver != null ? await InitializeCrawler(driver, url, 30, 0.5m) : null;
            //driver = null;
            //driver ??= await InitializeCrawler(driver, url, 30, 0.5m);
            if (driver != null) 
            {
                var titleDivElement = driver.FindElement(By.CssSelector("div[class*='Titles_titles']"));
                var titles = titleDivElement.Text;
                Console.WriteLine($"Found {url}");
                var listTitle = titles.Trim().Split("\r\n");

                Dictionary<string, string> name = new Dictionary<string, string>();
                if (listTitle != null)
                {
                    if (listTitle.Length > 0 && !String.IsNullOrEmpty(listTitle[0]))
                    {
                        name.Add("fa", listTitle[0]);
                    }
                    else
                    {
                        name.Add("fa", "");
                    }

                    if (listTitle.Length > 1 && !String.IsNullOrEmpty(listTitle[1]))
                    {
                        name.Add("en", listTitle[1]);
                    }
                    else
                    {
                        name.Add("en", "");
                    }

                    if (name["en"] != "")
                    {
                        result.Add(url, name["en"]);
                    }
                    else
                    {
                        result.Add(url, name["fa"]);
                    } 
                }

                driver.Quit();
            }

            return result;
        }
    }
}
