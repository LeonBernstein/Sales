using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sales.Common.Entities;
using Sales.Common.Interfaces.BL;
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

        public AuthController(
            ILogger<SalesBaseController> logger,
            IUsersLogic usersLogic
        ) : base(logger)
        {
            _usersLogic = usersLogic;
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
                if (!ModelState.IsValid) return BadRequest(ModelState);
                bool isUserValid = await _usersLogic.AreUsersDetailsValidAsync(model.UserId, model.Password);
                if (!isUserValid) return Unauthorized("Invalid credentials.");
                return NoContent();
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
                if (!ModelState.IsValid) return BadRequest(ModelState);
                bool isUserValid = await _usersLogic.AreUsersDetailsValidAsync(model.UserId, model.Password, model.OTP);
                if (!isUserValid) return Unauthorized("Invalid credentials.");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
