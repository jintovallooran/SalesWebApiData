using System.Net;
using System.Text.Json;

namespace SalesManagementWebAPI.Utilities
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UnauthorizedAccessException ex)
            {
                await Handle401Async(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await Handle401Async(context);
            }
            else
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Random random = new Random();
                DateTime now = DateTime.Now;


                string errorCode = $"ERR-{random.Next(1000, 10000)}{now.Day}{now.Month}{now.Year}";
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please try again later.",
                    Detailed = $"Please rise a ticket. Error Code :{errorCode}"
                };

                var errorDetails = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(errorDetails);
            }
        }
        // Make this method public to use it outside the middleware pipeline
        public async Task Handle401Async(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Unauthorized",
                Detailed = "Unauthorized. Your session may have expired. Please log in again."
            };

            var errorDetails = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(errorDetails);

        }

    }
}
