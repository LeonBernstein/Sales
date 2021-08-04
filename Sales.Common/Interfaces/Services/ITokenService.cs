
namespace Sales.Common.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId);
        bool IsTokenValid(string token, out string userId);
    }
}
