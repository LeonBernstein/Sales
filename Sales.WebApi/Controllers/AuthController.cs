using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sales.Common;
using Sales.Common.Entities;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.Services;
using Sales.WebApi.Models;
using System;
using System.Threading.Tasks;

namespace Sales.WebApi.Controllers
{
    [ApiController, Route("api/[controller]/")]
    [AllowAnonymous]
    public class AuthController : SalesBaseController
    {
        private readonly IUsersLogic _usersLogic;
        private readonly ITokenService _tokenService;
        private readonly WebAppSettings _appSettings;

        public AuthController(
            ILogger<SalesBaseController> logger,
            IUsersLogic usersLogic,
            ITokenService tokenService,
            WebAppSettings appSettings
        ) : base(logger)
        {
            _usersLogic = usersLogic;
            _tokenService = tokenService;
            _appSettings = appSettings;
        }


        [HttpGet, Route("GetLoginRequirements")]
        // If the user exists in the json file, then his phone number and is-OTP required will be returned.
        // The OTP can be the was sent to the user via SMS can be viewed in the console.
        public async Task<IActionResult> GetLoginRequirements(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("User ID must be provided.");
                }
                UserEntity user = await _usersLogic.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest("User ID is invalid.");
                }
                return Ok(new { user.IsOTPRequired, user.PhoneNumber });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost, Route("Login")]
        // Login method without OTP.
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                return await LoginHandlerAsync(model);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost, Route("LoginWithOTP")]
        // Login method with OTP (uses diffrent model then the regular login method).
        // The OTP can be the was sent to the user via SMS can be viewed in the console.
        public async Task<IActionResult> LoginWithOTP([FromBody] LoginUserWithOtpModel model)
        {
            try
            {
                return await LoginHandlerAsync(model);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // DRY login method that handles both OTP and non OTP validation.
        private async Task<IActionResult> LoginHandlerAsync(LoginUserModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool isUserValid = model is LoginUserWithOtpModel modelWithOTP
                ? await _usersLogic.AreUsersDetailsValidAsync(model.UserId, model.Password, modelWithOTP.OTP)
                : await _usersLogic.AreUsersDetailsValidAsync(model.UserId, model.Password);
            if (!isUserValid) return Unauthorized("Invalid credentials.");
            string token = _tokenService.GenerateToken(model.UserId);
            Response.Cookies.Append(_appSettings.AuthCookieName, token, GetAuthCookieOptions(_appSettings.AuthTokenExpInHours));
            return NoContent();
        }

        // Generates cookie options for the token.
        internal static CookieOptions GetAuthCookieOptions(int expiresInHours)
        {
            return new CookieOptions()
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddHours(expiresInHours),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };
        }
    }
}
