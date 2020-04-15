using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CityInfo.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddMvcOptions(o =>
           {
               o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
               // o.OutputFormatters.RemoveType(typeof(SystemTextJsonOutputFormatter));
           }).AddNewtonsoftJson();
            //.AddJsonOptions(jsonOptions =>
            //     {
            //         jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            //     });
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