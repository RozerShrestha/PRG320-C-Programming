using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessManagementSystem.Repositories
{
    public class TokenRepository : ITokenService
    {
        private const double EXPIRY_DURATION_DAY = 1;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(ILogger<TokenRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public JwtSecurityToken BuildToken(string key, string issuer, LoginResponseDto response)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentException("Key is required", nameof(key));
                
                if (string.IsNullOrEmpty(issuer))
                    throw new ArgumentException("Issuer is required", nameof(issuer));
                
                if (response == null)
                    throw new ArgumentNullException(nameof(response));

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, response.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Email, response.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, response.RoleDescription ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, response.Role ?? string.Empty)
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                
                var tokenDescriptor = new JwtSecurityToken(
                    issuer: issuer,
                    audience: issuer,
                    claims: claims,
                    expires: DateTime.Now.AddDays(EXPIRY_DURATION_DAY),
                    signingCredentials: credentials);

                _logger.LogInformation($"JWT token created for user: {response.UserName}");
                return tokenDescriptor;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error building JWT token: {ex.Message}");
                throw;
            }
        }

        public bool ValidateToken(string key, string issuer, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentException("Key is required", nameof(key));
                
                if (string.IsNullOrEmpty(issuer))
                    throw new ArgumentException("Issuer is required", nameof(issuer));
                
                if (string.IsNullOrEmpty(token))
                    throw new ArgumentException("Token is required", nameof(token));

                var mySecret = Encoding.UTF8.GetBytes(key);
                var mySecurityKey = new SymmetricSecurityKey(mySecret);
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);

                _logger.LogInformation("Token validation successful");
                return true;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning($"Token validation failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error validating token: {ex.Message}");
                return false;
            }
        }
    }
}
