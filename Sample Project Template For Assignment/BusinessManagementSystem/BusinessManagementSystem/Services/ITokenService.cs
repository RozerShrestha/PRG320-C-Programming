using BusinessManagementSystem.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Services
{
    public interface ITokenService
    {
        JwtSecurityToken BuildToken(string key, string issuer, LoginResponseDto user);
        bool ValidateToken(string key, string issuer, string token);
    }
}
