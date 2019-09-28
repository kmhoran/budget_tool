using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetApi.Common.Models;
using Extentions.Config;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using HistoricSheet.Common.Models;
using HistoricSheet.Common.Interfaces;

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
                builder.AddUserSecrets<GoogleServiceAccount>();
                builder.AddUserSecrets<MonthSheetDetails>();
                builder.AddUserSecrets<HistoricSheetDetails>();
            }

            builder.AddDockerSecrets();

            Configuration = builder.Build();

            var services = AppServices.GetProvider(Configuration);

            services.GetService<GoogleServiceAccount>();
            services.GetService<MonthSheetDetails>();

            try
            {
                var monthRepo = services.GetService<IMonthSheetRepository>();
                var monthService = services.GetService<IMonthSheetService>();
                var historicService = services.GetService<IHistoricSheetService>();


                var close = monthService.CloseMonth();
                if (!close.Success)
                {
                    // replace with some real error
                    Console.WriteLine(close.Exception.Message);
                    return;
                }

                var updateHistoric = historicService.AppendRecordsToHistoricLedger(close.Data.Transactions);
                if (!updateHistoric.Success)
                {
                    Console.WriteLine(updateHistoric.Exception.Message);
                    return;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Execution failed.");
                Console.WriteLine(e);
            }

        }
    }
}
