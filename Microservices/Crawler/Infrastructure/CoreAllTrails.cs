using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


namespace Crawler.Infrastructure
{
    public class CoreAllTrails
    {
        private ChromeDriver Driver
        {
            get
            {
                var chromeOption = new ChromeOptions();
                //chromeOption.AddArguments("headless");
                chromeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Default;
                chromeOption.PlatformName = "windows";
                chromeOption.BrowserVersion = "stable";
                chromeOption.AddArguments("start-maximized"); // open Browser in maximized mode
                chromeOption.AddArguments("--disable-extensions"); // disabling extensions
                chromeOption.AddArguments("--disable-gpu"); // applicable to windows os only
                chromeOption.AddArguments("--disable-dev-shm-usage"); // overcome limited resource problems
                chromeOption.AddArguments("--no-sandbox"); // Bypass OS security model
                //chromeOption.BinaryLocation = "D:\\TestProjects\\chrome-headless-shell-win64\\chrome-headless-shell.exe";
                //chromeOption.AddArguments("headless");
                //chromeOption.AddArguments(new List<string>() { "no-sandbox", "disable-gpu" });
                chromeOption.AddArguments("disable-infobars"); // disabling infobars
                //chromeOption.AddExcludedArgument("enable-automation");
                chromeOption.AddAdditionalChromeOption("useAutomationExtension", false);
                //chromeOption.Proxy = null;
                //chromeOption.AddArguments("--disable-logging");
                //chromeOption.AddArguments("--silent");
                //chromeOption.AddArguments("--log-level=3");
                chromeOption.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
                chromeOption.AddArgument("user-data-dir=C:\\Users\\amine\\AppData\\Local\\Google\\Chrome\\User Data");

                var chromeService = ChromeDriverService.CreateDefaultService();
                //chromeService.EnableVerboseLogging = false;
                //chromeService.DisableBuildCheck = false;
                //chromeService.SuppressInitialDiagnosticInformation = true;
                //chromeService.HideCommandPromptWindow = true;
                return new ChromeDriver(chromeService, chromeOption);
            }
        }

        public async Task Crawl(string url)
        {
            var driver = Driver;
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            driver.Navigate().GoToUrl(url);
            var cookies = driver.Manage().Cookies.AllCookies;
            var cookie = cookies.FirstOrDefault(q => q.Name.ToLower().Equals("refresh_token"));
            if (cookie == null)
            {
                driver.Navigate().GoToUrl("https://www.alltrails.com/login");
                
                await Task.Delay(5000);

                // Find the username and password fields and enter the credentials
                //driver.FindElement(By.Id("userEmail")).SendKeys("");
                //driver.FindElement(By.Id("userEmail")).SendKeys("ali.asadi82@gmail.com");
                //driver.FindElement(By.Id("userPassword")).SendKeys("");
                //driver.FindElement(By.Id("userPassword")).SendKeys("a0352amin");

                // Find the button with type="submit" using XPath
                IWebElement submitButton = driver.FindElement(By.XPath("//button[@type='submit']"));

                // Optionally, you can click the button
                submitButton.Click();
                
                await Task.Delay(5000);

                driver.Navigate().GoToUrl("https://www.alltrails.com/canada");
            }

            for (int i = 0; i < 10; i++)
            {
                IWebElement showMoreButton = driver.FindElement(By.XPath("//button[@data-testid='show-more-button']"));

                Actions actions = new Actions(driver);
                actions.MoveToElement(showMoreButton);
                actions.Perform();

                showMoreButton.Click();

                await Task.Delay(3000);
            }


            // Close the browser
            driver.Quit();

        }
    }
}
