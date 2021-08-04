using Microsoft.Extensions.Logging;
using Sales.Common.Interfaces.Services;
using System.Threading.Tasks;

namespace Sales.Services
{
    public class FakeSmsService : ISmsService
    {
        private readonly ILogger _logger;

        public FakeSmsService(ILogger<FakeSmsService> logger) => _logger = logger;

        public void SendSmsInBackground(string phoneNumber, string message)
        {
            Task.Run(() =>
            {
                string msg = $"SMS was sent to: {phoneNumber}, with message: \"{message}\"";
                _logger.LogInformation(msg);
            });
        }
    }
}
