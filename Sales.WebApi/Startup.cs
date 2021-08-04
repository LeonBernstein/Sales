using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sales.Common;
using Sales.Common.Entities;
using Sales.Config;
using Sales.WebApi.MiddleWares;
using Sales.WebApi.Models;

namespace Sales.WebApi
{
    public class Startup
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logging => logging.AddConsole());

            services.AddControllers(options => options.Filters.Add<UsersAuthorizationFilter>())
                .AddControllersAsServices();

            // Non WebApi DI is implemented in a dedicated assembly so that future startup projects could use the same configurations and also the current startup project won't need to depend on other logical assemblies like BL.
            DependencyInjectionConfig.ConfigureDI(services, _configuration);

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
