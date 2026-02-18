using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesManagement.Domain.Constants;
using SalesManagement.Domain.Models.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalesManagement.Utilities.Helpers
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        string jwtKey = "Jwt:Key";
        string issuer = "Jwt:Issuer";
        string audience = "Jwt:Audience";
        public TokenService(IConfiguration _config)
        {
            config = _config;
        }
        public Task<string> GenerateTokenAsync(UserClaimModel claim)
        {

            var claims = new[]
             {
                new Claim(ClaimTypes.Name, claim.Username??""),
                new Claim(ClaimConstants.UserId, claim.UserId.ToString()??"")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[jwtKey] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireInMinRaw = config["TokenExpireInMin"] ?? "";
            int expireInMin = int.TryParse(expireInMinRaw?.ToString(), out var n) ? n : 60;
            var token = new JwtSecurityToken(
                issuer: config[issuer],
                audience: config[audience],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireInMin),
                signingCredentials: creds);
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<string> GenerateRefreshTokenAsync(UserClaimModel claim)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Task.FromResult(Convert.ToBase64String(randomNumber));
            }

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config[issuer],
                ValidAudience = config[audience],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[jwtKey]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }

            throw new SecurityTokenException("Invalid token");
        }

        public async Task<UserClaimModel> GetClaimFromPrincipal(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimConstants.UserId)?.Value;
            var CompanyCode = principal.FindFirst(ClaimConstants.CompanyCode)?.Value;
            CompanyCode = CompanyCode != null ? CompanyCode : "0";

            var claim = new UserClaimModel()
            {
                Username = principal?.Identity?.Name,
                UserId = userId != null ? long.Parse(userId) : 0
            };
            return claim;

        }
    }
}
