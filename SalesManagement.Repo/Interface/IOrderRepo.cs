using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.Order;

namespace SalesManagement.Repo.Interface
{
    public interface IOrderRepo
    {
        Task<NotifyModel> SaveOrderDetailsAsync(OrderModel model);
        Task<IEnumerable<OrderBaseModel>> GetOrderNoAsync();
        Task<IEnumerable<OrderModel>> GetOrderListAsync(SearchModel model);
        Task<IEnumerable<ItemModel>> GetOrderItemListAsync(OrderModel model);
    }
}