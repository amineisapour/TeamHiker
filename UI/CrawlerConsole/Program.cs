using CrawlerConsole.Cache;
using CrawlerConsole.Domain;
using CrawlerConsole.Helper;
using CrawlerConsole.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CrawlerConsole
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = CreateServices(args);
            var url = "https://www.alltrails.com/canada";

            CoreAllTrails app = services.GetRequiredService<CoreAllTrails>();
            //await app.GetLinhCrawl(url);
            await app.GetPageCrawl();

            //var url1 = "https://www.tripadvisor.ca/Search?q=adventure&geo=1&ssrc=A&searchNearby=false&searchSessionId=001e1bd744db0c18.ssid&offset=0";
        }

        private static ServiceProvider CreateServices(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(GoogleSheetsHelper))
                .AddSingleton<GoogleSheet>()
                .AddSingleton<CoreAllTrails>()
                .AddSingleton<CacheService>()
                .AddScoped<IConfiguration>(_ => configuration)
                .BuildServiceProvider();

            return serviceProvider;
        }

    }
}
