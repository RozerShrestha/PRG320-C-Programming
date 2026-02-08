using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Repositories
{
    public class TokenRepository : ITokenService
    {
        private const double EXPIRY_DURATION_DAY = 1;
        public JwtSecurityToken BuildToken(string key, string issuer, LoginResponseDto response)
        {
            var claims = new[] {
            new Claim(ClaimTypes.Name, response.UserName),
            new Claim(ClaimTypes.Email,response.Email),
            new Claim(ClaimTypes.Role, response.RoleDescription),
            new Claim(ClaimTypes.NameIdentifier, response.Role)
        };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddDays(EXPIRY_DURATION_DAY), signingCredentials: credentials);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return tokenDescriptor;
        }
        public bool ValidateToken(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
