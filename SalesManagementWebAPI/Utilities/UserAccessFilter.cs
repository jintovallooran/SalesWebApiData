using Microsoft.Extensions.Configuration;
using SalesManagement.Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SalesManagement.Repo.Interface;

namespace SalesManagement.Utilities.Helpers
{
    public class UserAccessFilter : Attribute, IAsyncActionFilter
    {
        public string? PageUrl { get; set; }
        public IConfiguration? Configuration { get; }
        public IAdminRepo? adminRepo;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {

                var url = context.HttpContext.Request.Headers["user-validation"];
                if (!string.IsNullOrEmpty(url))
                {
                    var controllerUsingThisAttribute = ((dynamic)context.Controller);
                    adminRepo = controllerUsingThisAttribute.GetAdminRepository();
                    var test = Convert.ToString(url).Substring(5);
                    var base64EncodedBytes = System.Convert.FromBase64String(test);
                    var urlData = System.Text.Encoding.UTF8.GetString(base64EncodedBytes) ?? "";
                    NotifyModel result = new NotifyModel();

                    result = await adminRepo.ValidateUserScreen(urlData);

                    if ((result.MessageType == "success" || urlData == "/web-app/home") && urlData != "/web-app/test")
                        await next();
                    else
                        context.Result = new ContentResult { Content = "Invalid Page", StatusCode = 443 };

                }
                else
                {
                    await next();
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
