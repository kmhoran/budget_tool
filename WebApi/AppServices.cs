using System;
// using SheetApi.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SheetApi.Common.Interfaces;
using SheetApi.Services;
using MonthSheet.Common.Interfaces;
using MonthSheet.Repositories;
using MonthSheet.Services;
using HistoricSheet.Common.Interfaces;
using HistoricSheet.Repositories;
using HistoricSheet.Services;
using YearSheet.Common.Interfaces;
using YearSheet.Repositories;
using YearSheet.Services;
// using HistoricSheet.Common.Models;
// using HistoricSheet.Common.Interfaces;
// using YearSheet.Common.Models;
// using YearSheet.Common.Interfaces;

namespace WebApi
{
    public static class AppServices
    {
        public static IServiceCollection SetServices(IServiceCollection services)
        {

            services.AddSingleton<ISheetApiService, SheetApiService>()
            .AddScoped<IMonthSheetRepository, MonthSheetRepository>()
            .AddScoped<IMonthSheetService, MonthSheetService>()
            .AddScoped<IHistoricSheetRepository, HistoricSheetRepository>()
            .AddScoped<IHistoricSheetService, HistoricSheetService>()
            .AddScoped<IYearSheetRepositoryFactory, YearSheetRepositoryFactory>()
            .AddScoped<IYearSheetService, YearSheetService>();

            // var googleServiceAccount = config.GetSection("GoogleServiceAccount");
            // var monthSheetDetails = config.GetSection("MonthSheetDetails");
            // var historicSheetDetails = config.GetSection("HistoricSheetDetails");
            // var yearSheetDetails = config.GetSection("YearSheetDetails");

            // services.Configure<GoogleServiceAccount>(googleServiceAccount)
            // .Configure<MonthSheetDetails>(monthSheetDetails)
            // .Configure<HistoricSheetDetails>(historicSheetDetails)
            // .Configure<YearSheetDetails>(yearSheetDetails);

            return services;
        }
    }
}