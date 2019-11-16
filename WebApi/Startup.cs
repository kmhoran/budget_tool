using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoricSheet.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Models;
using SheetApi.Common.Models;
using YearSheet.Common.Models;
using MongoTest.Repositories;
using WebApi.Authentikate;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services = AppServices.SetServices(services);

            services.AddSingleton<IMongoTestRepository, MongoTestRespository>();

            services.Configure<GoogleServiceAccount>(Configuration.GetSection(nameof(GoogleServiceAccount)))
            .Configure<MonthSheetDetails>(Configuration.GetSection(nameof(MonthSheetDetails)))
            .Configure<HistoricSheetDetails>(Configuration.GetSection(nameof(HistoricSheetDetails)))
            .Configure<YearSheetDetails>(Configuration.GetSection(nameof(YearSheetDetails)));

            services.Configure<BookstoreDatabaseSettings>(
                Configuration.GetSection(nameof(BookstoreDatabaseSettings)));
            services.AddSingleton<IBookstoreDatabaseSettings>(
                sp => sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // services.AddHttpClient();
            services.AddHttpClient<IAuthentikateService, AuthentikateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath);
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
