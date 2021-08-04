using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sales.Common;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.Services;
using Sales.WebApi.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.WebApi.MiddleWares
{
    public class UsersAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger _logger;
        private readonly IUsersLogic _usersLogic;
        private readonly ITokenService _tokenService;

        public UsersAuthorizationFilter(
            ILogger<UsersAuthorizationFilter> logger,
            IUsersLogic usersLogic,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _usersLogic = usersLogic;
            _tokenService = tokenService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                if (context.ActionDescriptor.EndpointMetadata.Any(item => item is IAllowAnonymous)) return;
                HttpRequest request = context.HttpContext.Request;
                if (!request.Cookies.TryGetValue(Globals.AUTH_COOKIE_NAME, out string token)
                  || !_tokenService.IsTokenValid(token, out string userId)
                  || !await _usersLogic.IsUserExistsAsync(userId)
                )
                {
                    context.Result = Unauthorized();
                }
                else
                {
                    string newToken = _tokenService.GenerateToken(userId);
                    HttpResponse response = context.HttpContext.Response;
                    response.Cookies.Append(Globals.AUTH_COOKIE_NAME, newToken, AuthController.GetAuthCookieOptions());
                }
            }
            catch (Exception e)
            {
                context.Result = Unauthorized();
                _logger.LogError(e, e.Message);
            }
        }

        private static IActionResult Unauthorized()
        {
            return new UnauthorizedObjectResult("Unauthorized");
        }
    }
}
