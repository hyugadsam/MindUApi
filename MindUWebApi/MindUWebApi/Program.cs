using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MindUWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
}
