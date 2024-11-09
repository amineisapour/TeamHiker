using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrawlerConsole.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using CrawlerConsole.Interfaces;
using CrawlerConsole.Mapper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json.Serialization;
using System.Runtime;

namespace CrawlerConsole.Infrastructure
{
    public class CoreAllTrails
    {
        private readonly SettingsModel? _settings;
        private readonly GoogleSheet _googleSheet;

        public CoreAllTrails(IConfiguration configuration, GoogleSheet googleSheet)
        {
            _settings = configuration.GetSection("Settings").Get<SettingsModel>();
            _googleSheet = googleSheet;
        }

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

        public async Task GetLinhCrawl(string url)
        {
            if(_settings ==  null)
            {
                return;
            }
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

            int count = 99;
            //count = 2;
            for (int i = 0; i < count; i++)
            {

                //var element = driver.FindElement(By.XPath("//a[contains(@class, 'TopResults_resultName') and .//span[contains(@class, 'TopResults_desktopRanking') and contains(text(), '#40 -')]]"));

                IWebElement showMoreButton = driver.FindElement(By.XPath("//button[@data-testid='show-more-button']"));

                Actions actions = new Actions(driver);
                actions.MoveToElement(showMoreButton);
                actions.Perform();

                showMoreButton.Click();

                await Task.Delay(5000);
            }

            var linkElements = driver.FindElements(By.XPath("//a[contains(@class, 'TopResults_resultName')]"));
            foreach (var linkElement in linkElements)
            {
                //string href = linkElement.GetAttribute("href");
                //string linkText = linkElement.Text;

                AllTrailsLinkModel item = new AllTrailsLinkModel
                {
                    Link = linkElement.GetAttribute("href"),
                    Title = linkElement.Text
                };
                
                var result = await _googleSheet.WriteGoogleSheetDataAsync(_settings.GoogleSheet.Name, AllTrailsLinkMapper.MapToRangeData(item));
            }


            // Close the browser
            driver.Quit();

        }

        public async Task GetPageCrawl()
        {
            if(_settings ==  null)
            {
                return;
            }
            var dataList = await _googleSheet.GetGoogleSheetDataAsync();
            if (dataList != null)
            {
                List<AllTrailsLinkModel> rows = AllTrailsLinkMapper.MapFromRangeData(dataList);
                foreach (var row in rows)
                {
                    var indexList = row.Title.Replace("#", "").Split('-');
                    int index = int.Parse(indexList[0].Trim());
                    if (index >= _settings.GoogleSheet.StartIndex)
                    {
                        await PageCrawl(row.Link);
                    }
                }
            }
        }

        private async Task PageCrawl(string url)
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

                driver.Navigate().GoToUrl(url);
            }

            bool findBtn = false;
            int maxReviews = 33;
            //maxReviews = 1;
            int index = 0;
            do
            {
                //var divElement = driver.FindElement(By.XPath("//div[contains(text(), 'Showing results 1 -')]"));
                //string divText = divElement.Text;
                //var dataText = divText.Replace("<!-- -->", "").Trim().Split("of ");
                //var cnt = dataText[1].Trim();
                try
                {
                    IWebElement showMoreButton = driver.FindElement(By.XPath("//button[@data-testid='trail-reviews-show-more']"));

                    Actions actions = new Actions(driver);
                    actions.MoveToElement(showMoreButton);
                    actions.Perform();

                    showMoreButton.Click();

                    await Task.Delay(5000);

                    findBtn = true;
                    index++;
                    if (index == maxReviews)
                    {
                        findBtn = false;
                    }
                }
                catch
                {
                    findBtn = false;
                }

            } while (findBtn);

            string addressMap = "";

            var scriptTagBreadcrumbList = driver.FindElement(By.XPath("//div[contains(@class, 'BreadcrumbSearchBar_container')]/script[@type='application/ld+json']"));
            string scriptBreadcrumbListContent = scriptTagBreadcrumbList.GetAttribute("innerHTML");
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            BreadcrumbList breadcrumbList = JsonConvert.DeserializeObject<BreadcrumbList>(scriptBreadcrumbListContent.Replace("\"@", "\""), settings);
            foreach (var item in breadcrumbList.ItemListElement)
            {
                if(addressMap != "")
                {
                    addressMap = addressMap + " / ";
                }
                addressMap = addressMap + item.Name;
            }

            var scriptTagList = driver.FindElements(By.XPath("//div[contains(@class, 'MO7UyWM1JXXtcVuCt6leqg==')]/div/script[@type='application/ld+json']"));
            bool isExistSheet = false;
            foreach (var scriptTag in scriptTagList)
            {
                try
                {
                    string scriptContent = scriptTag.GetAttribute("innerHTML");
                    Review review = JsonConvert.DeserializeObject<Review>(scriptContent.Replace("\"@", "\""), settings);
                    //JObject json = JObject.Parse(scriptContent);
                    //Review review = JsonConvert.DeserializeObject<Review>(json.ToString());
                    //string name = json["author"]["name"].ToString();
                    if (review != null)
                    {
                        IWebElement reviewDate = driver.FindElement(By.XPath($"//span[@data-testid='{review.Author.Name}-secondary-text']"));

                        AllTraiReviewModel model = new AllTraiReviewModel
                        {
                            Address = review.ItemReviewed.Address.AddressLocality,
                            ReviewBody = review.ReviewBody,
                            ReviewRatingValue = review.ReviewRating.RatingValue.ToString(),
                            ReviewDate = reviewDate.Text.Trim(),
                            ReviewAuthor = review.Author.Name,
                            PageLink = "https://www.alltrails.com" + review.ItemReviewed.Id,
                            PageTitle = review.ItemReviewed.Name.Trim(),
                            PageAddressMap = addressMap
                        };

                        // create sheet
                        if (!isExistSheet)
                        {
                            isExistSheet = await _googleSheet.CreateNewGoogleSheet(model.PageTitle);
                            AllTraiReviewModel headerModel = new AllTraiReviewModel
                            {
                                Address = "Address",
                                ReviewBody = "Review Body",
                                ReviewRatingValue = "Review Rating Value",
                                ReviewDate = "Review Date",
                                ReviewAuthor = "Review Author",
                                PageLink = "Page Link",
                                PageTitle = "Page Title",
                                PageAddressMap = "Page Address Map"
                            };
                            await _googleSheet.WriteGoogleSheetDataAsync(model.PageTitle, AllTraiReviewMapper.MapToRangeData(headerModel));
                        }
                        var result = await _googleSheet.WriteGoogleSheetDataAsync(model.PageTitle, AllTraiReviewMapper.MapToRangeData(model));
                    }
                }
                catch { }
            }

            //AllTraiReviewMapper

            // Close the browser
            //driver.Close();
            driver.Quit();
        }


    }
}
