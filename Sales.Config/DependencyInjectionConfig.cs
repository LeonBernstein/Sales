using Microsoft.Extensions.DependencyInjection;
using Sales.BL;
using Sales.Common.Exceptions;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.DAL;
using Sales.Common.Interfaces.Services;
using Sales.DAL;
using Sales.Services;

namespace Sales.Config
{
    /// <summary>
    /// This class handles DI configurations that are not related to WebApi project.
    /// </summary>
    static public class DependencyInjectionConfig
    {
        static private bool _isDIConfigured = false;

        static public IServiceCollection ConfigureDI(IServiceCollection services)
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
            _isDIConfigured = true;
            return services;
        }
    }
}
