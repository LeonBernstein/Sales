using Sales.Common.Entities;
using Sales.Common.Interfaces.DAL;
using Sales.Common.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.DAL
{
    public class UsersEngine : BaseSalesEngine, IUsersEngine
    {
        public UsersEngine(IJsonFileHandlerService jsonFileHandler)
            : base(jsonFileHandler) { }

        public async Task<UserEntity> GetUserByIdAsync(string userId)
        {
            List<UserEntity> users = await GetItemsAsync<UserEntity>(EntityTypes.Users);
            return users.FirstOrDefault(user => user.UserId == userId);
        }

        public async Task<bool> IsUserExistsAsync(string userId)
        {
            List<UserEntity> users = await GetItemsAsync<UserEntity>(EntityTypes.Users);
            return users.Any(user => user.UserId == userId);
        }
    }
}
