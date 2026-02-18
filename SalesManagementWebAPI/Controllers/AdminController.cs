using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagement.Domain.Models.User;
using SalesManagement.Repo.Interface;
using SalesManagement.Utilities.Helpers;

namespace SalesManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        public readonly IAdminRepo adminRepo;
        public AdminController(IAdminRepo _adminRepo)
        {
            adminRepo = _adminRepo;
        }
        [NonAction]
        public IAdminRepo GetAdminRepository() => adminRepo;


        [HttpPost("GetScreenListAsync")]
        public async Task<IActionResult> GetScreenListAsync()
        => Ok(await adminRepo.GetMenuListAsync());

        [HttpPost("GetItemListAsync")]
        [UserAccessFilter]
        public async Task<IActionResult> GetItemListAsync()
        => Ok(await adminRepo.GetItemListAsync());

        [HttpPost("GetVendorListAsync")]
        public async Task<IActionResult> GetVendorListAsync()
        => Ok(await adminRepo.GetVendorListAsync());

        [HttpPost("GetUserListAsync")]
        public async Task<IActionResult>  GetUserListAsync(UserModel model)
        => Ok(await adminRepo.GetUserListAsync(model.RoleId));

    }
}
