using Sales.Common;
using Sales.Common.Interfaces.Services;
using System;
using System.Text;

namespace Sales.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOTP()
        {
            StringBuilder sb = new();
            Random random = new();
            for (int i = 0; i < Globals.OPT_LENGTH; i++)
            {
                int randomNumber = random.Next(0, 9);
                sb.Append(randomNumber);
            }
            return sb.ToString();
        }
    }
}
