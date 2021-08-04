using Sales.Common;
using Sales.Common.Interfaces.Services;
using System;
using System.Text;

namespace Sales.Services
{
    public class OtpService : IOtpService
    {
        private readonly WebAppSettings _appSettings;

        public OtpService(WebAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateOTP()
        {
            StringBuilder sb = new();
            Random random = new();
            for (int i = 0; i < _appSettings.OPTLength; i++)
            {
                int randomNumber = random.Next(0, 9);
                sb.Append(randomNumber);
            }
            return sb.ToString();
        }
    }
}
