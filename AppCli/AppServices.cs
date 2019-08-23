using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using MonthSheet.Repositories;
using MonthSheet.Services;
using Sheet.Common;
using Sheet.Common.Interfaces;
using Sheet.Common.Models;

namespace AppCli
{
    public class AppServices
    {

        public static ServiceProvider GetProvider(IConfigurationRoot config)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<ISheetRepository, SheetRepository>()
            .AddScoped<IMonthSheetRepository, MonthSheetRepository>()
            .AddScoped<IMonthSheetService, MonthSheetService>();

            // Configure secrets
            collection.Configure<GoogleServiceAccount>(config.GetSection(nameof(GoogleServiceAccount)))
            .Configure<MonthSheetDetails>(config.GetSection(nameof(MonthSheetDetails)));

            collection.AddOptions();

            return collection.BuildServiceProvider();
        }
    }
}