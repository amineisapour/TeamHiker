using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
//using Serilog;
//using System;
//using System.IO;

namespace AuthenticationMicroservice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
            //    .WriteTo.Console()
            //    .CreateBootstrapLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                //        .ReadFrom.Configuration(hostingContext.Configuration))
            ;
    }
}
