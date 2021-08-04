using Sales.Common.Entities;
using System.Threading.Tasks;

namespace Sales.Common.Interfaces.DAL
{
    public interface IUsersEngine
    {
        Task<UserEntity> GetUserByIdAsync(string userId);
    }
}
