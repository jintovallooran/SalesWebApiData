using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace SalesManagementWebAPI.Utilities
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            string key = "Jwt:Key";
            string issuer = "Jwt:Issuer";
            string audience = "Jwt:Audience";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero, // Optional: Ensures immediate expiration check without clock skew
                    ValidIssuer = configuration[issuer],
                    ValidAudience = configuration[audience],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[key]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;                    

                        var exceptionMiddleware = new ExceptionMiddleware(context.HttpContext.RequestServices.GetRequiredService<RequestDelegate>(),
                                                                          context.HttpContext.RequestServices.GetRequiredService<ILogger<ExceptionMiddleware>>());

                        // Call the Handle401Async method to handle the unauthorized response
                        await exceptionMiddleware.Handle401Async(context.HttpContext);
                    }
                };
            });

            return services;
        }
    }
}
