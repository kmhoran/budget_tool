using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sheet.Common.Interfaces;
using Sheet.Common.Models;
using Sheet.Common;
using Sheet.Common.Interfaces;
using Sheet.Common.Models;

namespace AppCli
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (isDevelopment)
            {
                builder.AddUserSecrets<GoogleConfig>();
            }

            Configuration = builder.Build();

            var services = AppServices.GetProvider(Configuration);

            services.GetService<GoogleConfig>();
            var sheetRepo = services.GetService<ISheetRepository>();
            sheetRepo.QuickStart();
        }
    }
}
