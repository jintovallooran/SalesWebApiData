using SalesManagement.Domain.Models.Login;
using System.Security.Claims;

namespace SalesManagement.Utilities.Helpers
{
    public interface ITokenService
    {
        Task<string> GenerateRefreshTokenAsync(UserClaimModel claim);
        Task<string> GenerateTokenAsync(UserClaimModel claim);
        Task<UserClaimModel> GetClaimFromPrincipal(ClaimsPrincipal principal);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    }
}