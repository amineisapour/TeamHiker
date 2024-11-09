using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Crawler.Infrastructure
{
    internal class Core3
    {
        static async Task Main(string[] args)
        {
            string link = "https://index.hu";

            List<string> allProxies = await FilterProxiesAsync();
            string newProxy = "allProxies";
            allProxies.RemoveAt(allProxies.Count - 1);

            IWebDriver driver = CreateProxyDriver(newProxy);
            await GetContentAsync(allProxies, driver, link);
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

        static IWebDriver CreateProxyDriver(string proxy)
        {
            var options = new ChromeOptions();
            options.AddArgument($"--proxy-server={proxy}");
            options.AddArgument("--headless"); // Run Chrome in headless mode
            return new ChromeDriver(options);
        }

        static async Task GetContentAsync(List<string> allProxies, IWebDriver driver, string link)
        {
            while (true)
            {
                try
                {
                    driver.Navigate().GoToUrl(link);
                    Console.WriteLine("Successfully accessed the link");

                    // Add your Selenium scraping code here

                    break; // Exit the loop if successful
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    driver.Quit();

                    if (allProxies.Count == 0)
                    {
                        Console.WriteLine("Proxies used up");
                        allProxies = await FilterProxiesAsync();
                    }

                    string newProxy = "allProxies";
                    allProxies.RemoveAt(allProxies.Count - 1);
                    driver = CreateProxyDriver(newProxy);
                    Console.WriteLine($"New proxy being used: {newProxy}");
                    await Task.Delay(1000);
                }
            }
        }
    }
}
