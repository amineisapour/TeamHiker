using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Infrastructure
{
    public class Core
    {

        public async Task<string> Crawl(string url)
        {
            var options = new LaunchOptions
            {
                Headless = false,
                //ExecutablePath = @"D:\TestProjects\chrome-headless-shell-win64\chrome-headless-shell.exe"
                ExecutablePath = "D:\\TestProjects\\chrome-win64\\chrome.exe",
                //Args = ["--start-fullscreen", "--window-size=1920,1040"]
                Args = ["--window-size=1920,1040"]
            };

            
            // Launch the browser
            await using var browser = await Puppeteer.LaunchAsync(options);

            await using var page = await browser.NewPageAsync();
            await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            //await page.SetUserAgentAsync("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.88 Safari/537.36");

            // Navigate to the product page

            //NavigationOptions opt = new NavigationOptions();
            //opt.Timeout = 0;
            //opt.WaitUntil = [WaitUntilNavigation.DOMContentLoaded];

            //var navigationPromise = await page.WaitForNavigationAsync(opt);

            // Set the headers
            await page.SetExtraHttpHeadersAsync(
                new Dictionary<string, string>
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET" },
                    { "Access-Control-Allow-Headers", "Content-Type" },
                    { "accept", "*/*" },
                    { "accept-encoding", "gzip, deflate" },
                    { "accept-language", "en,mr;q=0.9" },
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36" }
                });

            //await page.GoToAsync(url, 0, [WaitUntilNavigation.DOMContentLoaded]);
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle2);
            //await page.SetViewportAsync(new ViewPortOptions
            //{
            //    Width = 1920,
            //    Height = 1080,
            //    DeviceScaleFactor = 1,
            //});

            var html = await page.GetContentAsync();

            //var cooki = await page.GetCookiesAsync();

            //WaitForSelectorOptions selectorOptions = new WaitForSelectorOptions
            //{
            //    Timeout = 0
            //};

            //await page.WaitForSelectorAsync("div.all-results-section", selectorOptions);

            //// Evaluate JavaScript in the context of the page to get the desired elements
            //var divElements = await page.EvaluateExpressionAsync<string[]>(@"Array.from(document.querySelectorAll('div.kgrOn.o')).map(div => div.outerHTML);");

            //// Print the outer HTML of each div element
            //foreach (var div in divElements)
            //{
            //    var di = div;
            //    //Console.WriteLine(div);
            //}

            // Close the browser
            await browser.CloseAsync();
            return html;
        }
    }
}