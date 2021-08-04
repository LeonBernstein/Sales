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
            services.AddSingleton<IJsonFileHandlerService, JsonFileHandlerService>();
            services.AddSingleton<IOtpService, OtpService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ISmsService, FakeSmsService>();
            _isDIConfigured = true;
            return services;
        }
    }
}
