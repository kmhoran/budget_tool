using System;
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
using WebApi.Authentikate;

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

            return services;
        }
    }
}