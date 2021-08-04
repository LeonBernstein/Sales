using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.BL;
using Sales.Common;
using Sales.Common.Exceptions;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.DAL;
using Sales.Common.Interfaces.Services;
using Sales.DAL;
using Sales.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales.Config
{
    static public class DependencyInjectionConfig
    {
        static private bool _isDIConfigured = false;

        static public IServiceCollection ConfigureDI(IServiceCollection services, IConfiguration config)
        {
            if (_isDIConfigured)
            {
                throw new AppException("Dependency injection was already configured by \"DependencyInjectionConfig\".");
            }
            services.AddMemoryCache();
            services.AddTransient<IUsersLogic, UsersLogic>();
            services.AddTransient<IUsersEngine, UsersEngine>();
            services.AddTransient<ICustomersEngine, CustomersEngine>();
            services.AddTransient<ICustomersLogic, CustomersLogic>();
            services.AddTransient<IJsonFileHandlerService, JsonFileHandlerService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<ISmsService, FakeSmsService>();
            services.AddTransient<ITokenService, JwtTokenService>();
            services.AddTransient(options =>
            {
                AppSettings result = config.GetSection("AppSettings")
                    .Get<AppSettings>();

                bool isValid = Validator.TryValidateObject(result,
                    new ValidationContext(result),
                    new List<ValidationResult>(),
                    true
                 );
                if (!isValid)
                {
                    throw new AppException("Not all app settings are valid.");
                }
                return result;
            });
            _isDIConfigured = true;
            return services;
        }
    }
}
