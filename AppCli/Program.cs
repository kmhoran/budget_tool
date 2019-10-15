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
using YearSheet.Common.Models;
using YearSheet.Common.Interfaces;

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
                builder.AddUserSecrets<YearSheetDetails>();
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
                var yearService = services.GetService<IYearSheetService>();


                var close = monthService.CloseMonth();
                if (!close.Success)
                {
                    // replace with some real error
                    Console.WriteLine(close.Exception.Message);
                    Console.WriteLine(close.Exception.StackTrace);
                    return;
                }

                var updateHistoric = historicService.AppendRecordsToHistoricLedger(close.Data.Transactions);
                if (!updateHistoric.Success)
                {
                    Console.WriteLine(updateHistoric.Exception.Message);
                    Console.WriteLine(updateHistoric.Exception.StackTrace);
                    return;
                }

                var projectionsResponse = yearService.SaveCategoriesToYear(close.Data.Categories);
                if (!projectionsResponse.Success)
                {
                    Console.WriteLine(projectionsResponse.Exception.Message);
                    Console.WriteLine(projectionsResponse.Exception.StackTrace);
                    return;
                }

                var projectionUpdateResponse = monthService.UpdateCategoryProjections(projectionsResponse.Data);
                if (!projectionUpdateResponse.Success)
                {
                    Console.WriteLine(projectionUpdateResponse.Exception.Message);
                    Console.WriteLine(projectionUpdateResponse.Exception.StackTrace);
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
