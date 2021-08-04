using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sales.Common;
using Sales.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sales.Services
{
    public class JwtTokenService : ITokenService
    {
        private const string APP_NAME = "SalesApp";
        private const string SECURE_KEY = "eaQRTRexC09ffKPOcw9nVzxikSDaYCt6";

        private readonly ILogger _logger;

        public JwtTokenService(ILogger<JwtTokenService> logger) => _logger = logger;

        public string GenerateToken(string userId)
        {
            var currDate = DateTime.UtcNow;
            SigningCredentials signingKey = GenerateSigningKey();
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = APP_NAME,
                IssuedAt = currDate,
                NotBefore = currDate,
                Expires = currDate.AddHours(Globals.AUTH_TOKEN_EXP_IN_HOURS),
                SigningCredentials = signingKey,
            };
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public bool IsTokenValid(string token, out string userId)
        {
            userId = null;
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                if (tokenHandler.ReadToken(token) is not JwtSecurityToken) return false;
                SigningCredentials signingKey = GenerateSigningKey();
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuers = new string[] { APP_NAME, },
                    IssuerSigningKey = signingKey.Key,
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken _);
                //principal.Claims.
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        private static SigningCredentials GenerateSigningKey()
        {
            byte[] secureKeyBytes = Encoding.UTF8.GetBytes(SECURE_KEY);
            SymmetricSecurityKey symmetricSecurityKey = new(secureKeyBytes);
            SigningCredentials signingKey = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            return signingKey;
        }
    }
}
