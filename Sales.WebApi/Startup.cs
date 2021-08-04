using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sales.Common;
using Sales.Common.Entities;
using Sales.Common.Exceptions;
using Sales.Config;
using Sales.WebApi.MiddleWares;
using Sales.WebApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logging => logging.AddConsole());

            // The "UsersAuthorizationFilter" is responsible for athorizing access for non anonymous controllers..
            services.AddControllers(options => options.Filters.Add<UsersAuthorizationFilter>())
                .AddControllersAsServices();

            // Configures AppSettings bindings for AppSettings class.
            services.AddTransient(options =>
            {
                AppSettings result = _configuration.GetSection("AppSettings")
                    .Get<AppSettings>();

                bool isValid = Validator.TryValidateObject(result,
                    new System.ComponentModel.DataAnnotations.ValidationContext(result),
                    new List<ValidationResult>(),
                    true
                 );
                if (!isValid)
                {
                    throw new AppException("Not all app settings are valid.");
                }
                return result;
            });

            // Non WebApi DI is implemented in a dedicated assembly so that future startup projects could use the same configurations and also the current startup project won't need to depend on other logical assemblies like BL.
            DependencyInjectionConfig.ConfigureDI(services);

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<CustomerModel, CustomerEntity>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
