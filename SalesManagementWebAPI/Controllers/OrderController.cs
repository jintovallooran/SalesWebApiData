using Microsoft.AspNetCore.Mvc;
using SalesManagement.Domain.Models.Order;
using SalesManagement.Repo.Interface;
using SalesManagement.Repo.Repository;
using SalesManagement.Utilities.Helpers;

namespace SalesManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IOrderRepo orderRepo;
        public readonly IAdminRepo adminRepo;
        public OrderController(IOrderRepo _orderRepo,IAdminRepo _adminRepo)
        {
            orderRepo = _orderRepo;
            adminRepo = _adminRepo;
        }

        [NonAction]
        public IAdminRepo GetAdminRepository() => adminRepo;

        [HttpPost("SaveOrderDetailAsync")]
        public async Task<IActionResult> SaveOrderDetailAsync(OrderModel model)
        {
            return Ok(await orderRepo.SaveOrderDetailsAsync(model));
        }

        [HttpPost("GetOrderNoAsync")]
        public async Task<IActionResult> GetOrderNoAsync()
        {
            return Ok(await orderRepo.GetOrderNoAsync());
        }

        [HttpPost("GetOrderListAsync")]
        [UserAccessFilter]
        public async Task<IActionResult> GetOrderListAsync(SearchModel model)
        {
            return Ok(await orderRepo.GetOrderListAsync(model));
        }

        [HttpPost("GetOrderItemListAsync")]
        public async Task<IActionResult> GetOrderItemListAsync(OrderModel model)
        {
            return Ok(await orderRepo.GetOrderItemListAsync(model));
        }
    }
}
