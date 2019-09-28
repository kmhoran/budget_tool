using System;
using HistoricSheet.Common.Interfaces;
using HistoricSheet.Common.Models;
using HistoricSheet.Repositories;
using HistoricSheet.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using MonthSheet.Repositories;
using MonthSheet.Services;
using SheetApi.Common.Interfaces;
using SheetApi.Common.Models;
using SheetApi.Services;

namespace AppCli
{
    public class AppServices
    {

        public static ServiceProvider GetProvider(IConfigurationRoot config)
        {
            var collection = new ServiceCollection();
            collection
            .AddSingleton<ISheetApiService, SheetApiService>()
            .AddScoped<IMonthSheetRepository, MonthSheetRepository>()
            .AddScoped<IMonthSheetService, MonthSheetService>()
            .AddScoped<IHistoricSheetRepository, HistoricSheetRepository>()
            .AddScoped<IHistoricSheetService, HistoricSheetService>();

            // Configure secrets
            collection.Configure<GoogleServiceAccount>(config.GetSection(nameof(GoogleServiceAccount)))
            .Configure<MonthSheetDetails>(config.GetSection(nameof(MonthSheetDetails)))
            .Configure<HistoricSheetDetails>(config.GetSection(nameof(HistoricSheetDetails)));

            collection.AddOptions();

            return collection.BuildServiceProvider();
        }
    }
}