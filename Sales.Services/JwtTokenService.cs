using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sales.Common;
using Sales.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Sales.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;

        public JwtTokenService(ILogger<JwtTokenService> logger, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _logger = logger;
        }

        public string GenerateToken(string userId)
        {
            var currDate = DateTime.UtcNow;
            SigningCredentials signingKey = GenerateSigningKey();
            Dictionary<string, object> claims = new()
            {
                { JwtRegisteredClaimNames.NameId, userId.ToString() },
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
            };
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Claims = claims,
                Issuer = _appSettings.AppName,
                IssuedAt = currDate,
                NotBefore = currDate,
                Expires = currDate.AddHours(_appSettings.AuthTokenExpInHours),
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
                if (tokenHandler.ReadToken(token) is not JwtSecurityToken securityToken) return false;
                SigningCredentials signingKey = GenerateSigningKey();
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = new string[] { _appSettings.AppName, },
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = signingKey.Key,
                };
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken _);
                userId = securityToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value;
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        private SigningCredentials GenerateSigningKey()
        {
            byte[] secureKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecureKey);
            SymmetricSecurityKey symmetricSecurityKey = new(secureKeyBytes);
            SigningCredentials signingKey = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            return signingKey;
        }
    }
}
