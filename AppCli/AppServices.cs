using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            collection.AddScoped<ISheetRepository, SheetRepository>();
            collection.Configure<GoogleConfig>(config.GetSection(nameof(GoogleConfig)))
            .AddOptions();

            return collection.BuildServiceProvider();
        }
    }
}