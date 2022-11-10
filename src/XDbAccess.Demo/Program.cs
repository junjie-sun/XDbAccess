using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace XDbAccess.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    var otherConfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

                    webBuilder.UseConfiguration(otherConfig);

                    var envName = webBuilder.GetSetting("environment");

                    var nlogFileName = string.IsNullOrEmpty(envName) || envName.ToLower() == "production" ? "nlog.config" : $"nlog.{envName}.config";
                    var logger = NLogBuilder.ConfigureNLog($"{Directory.GetCurrentDirectory()}/{nlogFileName}").GetCurrentClassLogger();

                    logger.Debug($"Init finished. Environment={envName}");
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .UseNLog();
    }
}
