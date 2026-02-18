using Dapper;
using Microsoft.Extensions.Configuration;
using SalesManagement.Domain.Claim;
using SalesManagement.Domain.Constants;
using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.Common;
using SalesManagement.Domain.Models.Login;
using SalesManagement.Domain.Models.Order;
using SalesManagement.Domain.Models.User;
using SalesManagement.Domain.Models.Vendor;
using SalesManagement.Repo.Interface;
using SalesManagement.Utilities;
using System.Data;
using System.Security.Claims;

namespace SalesManagement.Repo.Repository
{
    public class AdminRepo : DBConnector, IAdminRepo
    {
        private readonly IClaimsAccessor claimsAccessor;
        public AdminRepo(IClaimsAccessor _claimsAccessor, IConfiguration configuration) : base(configuration)
        {
            claimsAccessor = _claimsAccessor;
        }

        #region Screen
        public async Task<IEnumerable<MenuItemModel>> GetMenuListAsync()
        {
            UserClaimModel claim = claimsAccessor.GetClaimValue();

            var parameters = new DynamicParameters();
            parameters.Add(DBConstants.Parameters.UserId, claim.UserId);

            var roles = (await GetAsync<RoleBaseModel>(DBConstants.Procedures.GetUserRole, parameters, CommandType.StoredProcedure)).Select(r => r.RoleId).ToHashSet();

            var menuList = new List<MenuItemModel> { new MenuItemModel { Label = "Dashboard", Icon = "pi pi-home", RouterLink = "/web-app/home" } };
            if (roles.Contains(1) || roles.Contains(2))
            {
                menuList.Add(CreateSalesMenu());
            }

            if (roles.Contains(1))
            {
                menuList.Add(CreateAdminMenu());
            }

            return menuList;
        }

        private static MenuItemModel CreateSalesMenu()
        {
            return new MenuItemModel
            {
                Label = "Sales",
                Icon = "pi pi-shopping-cart",
                Items = new List<MenuItemModel>
                    {
                        new MenuItemModel { Label = "New Order", Icon = "pi pi-plus", RouterLink = "/web-app/order/order-form" },
                        new MenuItemModel { Label = "Orders", Icon = "pi pi-list", RouterLink = "/web-app/order/order-list" }
                    }
            };
        }

        private static MenuItemModel CreateAdminMenu()
        {
            return new MenuItemModel
            {
                Label = "Administrator",
                Icon = "pi pi-box",
                Items = new List<MenuItemModel>
                    {
                        new MenuItemModel { Label = "User List", Icon = "pi pi-list", RouterLink = "/web-app/admin/user-list" }
                    }
            };
        }

        #endregion

        public async Task<IEnumerable<ItemModel>> GetItemListAsync()
        {
            UserClaimModel claim = claimsAccessor.GetClaimValue();

            var parameters = new DynamicParameters();
            return await GetAsync<ItemModel>(DBConstants.Procedures.GetItemList, parameters, CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<VendorModel>> GetVendorListAsync()
        {
            var parameters = new DynamicParameters();
            return await GetAsync<VendorModel>(DBConstants.Procedures.GetVendorList, parameters, CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<UserModel>> GetUserListAsync(int? role)
        {
            var parameters = new DynamicParameters();

            parameters.Add(DBConstants.Parameters.RoleId, role);
            return await GetAsync<UserModel>(DBConstants.Procedures.GetUserList, parameters, CommandType.StoredProcedure);

        }

        public async Task<NotifyModel> ValidateUserScreen(string? screen)
        {
            return new NotifyModel("success",2);
        }
    }

}
