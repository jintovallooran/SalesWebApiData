using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.Login;
using SalesManagement.Domain.Models.User;
using SalesManagement.Repo.Interface;
using SalesManagement.Utilities.Extensions;
using SalesManagement.Utilities.Helpers;

namespace SalesManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        public readonly IAuthRepo authRepo;
        private readonly ITokenService tokenService;
        public AuthController(IAuthRepo _authRepo, ITokenService _tokenService)
        {
            authRepo = _authRepo;
            tokenService = _tokenService;
        }

        [HttpPost("ValidateLoginAsync")]
        public async Task<IActionResult> ValidateLoginAsync(LoginModel model)
        {
            LoginResponseModel responseModel = new LoginResponseModel();
            try
            {
                model.Password = model.Password?.Encrypt();
                NotifyModel notifyModel = await authRepo.GetValidateLoginAsync(model);
                if (notifyModel.MessageType?.ToLower() == "success")
                {
                    UserClaimModel claim = new UserClaimModel()
                    {
                        UserId = string.IsNullOrEmpty(notifyModel.UserId) ? 0 : Convert.ToInt64(notifyModel.UserId),
                        Msg = notifyModel.Message,
                        Username = model?.UserName ?? "",
                    };
                    responseModel = new LoginResponseModel()
                    {
                        Token = await tokenService.GenerateTokenAsync(claim),
                        RefreshToken = await tokenService.GenerateRefreshTokenAsync(claim),
                        UserId = claim.UserId.ToString()
                    };
                }
                else
                {
                    responseModel.Message = notifyModel.Message;
                    responseModel.MessageType = notifyModel.MessageType;
                }

            }
            catch (Exception ex)
            {
                responseModel = new LoginResponseModel() { Message = ex.Message, MessageType = "error" };
            }

            return Ok(responseModel);
        }
    }
}
