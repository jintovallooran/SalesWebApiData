using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.Common;
using SalesManagement.Domain.Models.Order;
using SalesManagement.Domain.Models.User;
using SalesManagement.Domain.Models.Vendor;

namespace SalesManagement.Repo.Interface
{
    public interface IAdminRepo
    {
        Task<IEnumerable<MenuItemModel>> GetMenuListAsync();
        Task<IEnumerable<ItemModel>> GetItemListAsync();
        Task<IEnumerable<VendorModel>> GetVendorListAsync();
        Task<NotifyModel> ValidateUserScreen(string? screen);
        Task<IEnumerable<UserModel>> GetUserListAsync(int? role);
    }
}