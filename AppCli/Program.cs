using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetApi.Common.Models;
using Extentions.Config;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using HistoricSheet.Common.Models;

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

                // TODO: Remove Demo
                // var transactions = monthRepo.LoadTransactions();
                // foreach (var expense in transactions.Expenses)
                // {
                //     Console.WriteLine($"${expense.NetCost}\t\t{expense.Detail}");
                // }

                // TODO: REMOVE
                // var categories = monthRepo.LoadCategories();
                // foreach(var c in categories.Green.RawExpense)
                // {
                //     Console.WriteLine($"${c[0].ToString()}");
                // }

                // monthRepo.UpdateImbalance((decimal)123456.798, 26);

                var close = monthService.CloseMonth();
                if(!close.Success)
                {
                    // replace with some real error
                    Console.WriteLine(close.Exception);
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
