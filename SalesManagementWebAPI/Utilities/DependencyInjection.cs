using SalesManagement.Domain.Claim;
using SalesManagement.Repo.Interface;
using SalesManagement.Repo.Repository;
using SalesManagement.Utilities.Helpers;

namespace SalesManagementWebAPI.Utilities
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepoDependencyInjection(this IServiceCollection service)
        {
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            service.AddScoped<IClaimsAccessor, ClaimsAccessor>();
            service.AddScoped<IAuthRepo, AuthRepo>();
            service.AddScoped<IAdminRepo, AdminRepo>();
            service.AddScoped<IOrderRepo, OrderRepo>();

            service.AddScoped<ITokenService, TokenService>();
            return service;
        }
        }
}
