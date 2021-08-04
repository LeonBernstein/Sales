
namespace Sales.Common.Interfaces.Services
{
    public interface ISmsService
    {
        void SendSmsInBackground(string phoneNumber, string message);
    }
}
