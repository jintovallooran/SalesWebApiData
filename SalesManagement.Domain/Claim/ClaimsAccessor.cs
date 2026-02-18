using SalesManagement.Domain.Models.Login;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SalesManagement.Domain.Claim
{

    public interface IClaimsAccessor
    {
        UserClaimModel GetClaimValue(string? claimType = ClaimTypes.Name);
    }

    public class ClaimsAccessor : IClaimsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserClaimModel GetClaimValue(string? claimType = ClaimTypes.Name)
        {
            var userId = GetClaim("UserId");
            UserClaimModel claim = new UserClaimModel()
            {
                Username = GetClaim(ClaimTypes.Name),
                UserId = userId != null ? long.Parse(userId) : 0,
            };


            return claim;
        }

        private string? GetClaim(string claimType)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

    }

}
