using Sales.Common.Entities;
using System.Threading.Tasks;

namespace Sales.Common.Interfaces.BL
{
    public interface IUsersLogic
    {
        Task<UserEntity> GetUserByIdAsync(string userId);
        Task<bool> AreUsersDetailsValidAsync(string userId, string password, string otp = null);
        Task<bool> IsUserExistsAsync(string userId);
    }
}
