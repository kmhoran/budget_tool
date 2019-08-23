using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sheet.Common.Interfaces;
using Sheet.Common.Models;
using Extentions.Config;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;

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
            }

            builder.AddDockerSecrets();

            Configuration = builder.Build();

            var services = AppServices.GetProvider(Configuration);

            services.GetService<GoogleServiceAccount>();
            services.GetService<MonthSheetDetails>();

            try
            {
                var monthRepo = services.GetService<IMonthSheetRepository>();
                var transactions = monthRepo.LoadTransactions();

                // TODO: Remove Demo
                foreach (var expense in transactions.Expenses)
                {
                    Console.WriteLine($"${expense.NetCost}\t\t{expense.Detail}");
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
