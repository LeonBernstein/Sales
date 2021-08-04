using AutoMapper;
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

        public AuthController(
            ILogger<SalesBaseController> logger,
            IUsersLogic usersLogic,
            ITokenService tokenService
        ) : base(logger)
        {
            _usersLogic = usersLogic;
            _tokenService = tokenService;
        }


        [HttpGet, Route("GetLoginRequirements")]
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

        private async Task<IActionResult> LoginHandlerAsync(LoginUserModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool isUserValid = model is LoginUserWithOtpModel modelWithOTP
                ? await _usersLogic.AreUsersDetailsValidAsync(model.UserId, model.Password, modelWithOTP.OTP)
                : await _usersLogic.AreUsersDetailsValidAsync(model.UserId, model.Password);
            if (!isUserValid) return Unauthorized("Invalid credentials.");
            string token = _tokenService.GenerateToken(model.UserId);
            Response.Cookies.Append(Globals.AUTH_COOKIE_NAME, token, GetAuthCookieOptions());
            return NoContent();
        }
    }
}
