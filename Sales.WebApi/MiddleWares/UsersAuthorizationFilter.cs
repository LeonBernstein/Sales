using Microsoft.AspNetCore.Mvc.Filters;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.WebApi.MiddleWares
{
    public class UsersAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IUsersLogic _usersLogic;
        private readonly ITokenService _tokenService;

        public UsersAuthorizationFilter(IUsersLogic usersLogic, ITokenService tokenService)
        {
            _usersLogic = usersLogic;
            _tokenService = tokenService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //test = context;
        }
    }
}
