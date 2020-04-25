using CityInfo.API.Contexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CityInfo.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddMvcOptions(o =>
           {
               // o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
               // o.OutputFormatters.RemoveType(typeof(SystemTextJsonOutputFormatter));
           }).AddNewtonsoftJson();
            //.AddJsonOptions(jsonOptions =>
            //     {
            //         jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            //     });
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();

#endif
            var connectionString = _config["connectionStrings:cityInfoDbConnectionString"];
            services.AddDbContext<CityInfoContext>(o =>
            {
                o.UseSqlServer(connectionString);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseMvc();

            app.UseRouting();
        }
    }
}