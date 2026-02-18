using Dapper;
using Microsoft.Extensions.Configuration;
using SalesManagement.Domain.Claim;
using SalesManagement.Domain.Constants;
using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.Login;
using SalesManagement.Domain.Models.Order;
using SalesManagement.Repo.Interface;
using SalesManagement.Utilities;
using SalesManagement.Utilities.Extensions;
using System.Data;
using System.Reflection;

namespace SalesManagement.Repo.Repository
{
    public class OrderRepo : DBConnector, IOrderRepo
    {
        private readonly IClaimsAccessor claimsAccessor;
        public OrderRepo(IClaimsAccessor _claimsAccessor, IConfiguration configuration) : base(configuration)
        {
            claimsAccessor = _claimsAccessor;
        }

        public async Task<NotifyModel> SaveOrderDetailsAsync(OrderModel model)
        {

            UserClaimModel claim = claimsAccessor.GetClaimValue();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                var dt_TripList = model.Items?.ToDataTable();
                parameters.Add(DBConstants.Parameters.OrderId, model.OrderId);
                parameters.Add(DBConstants.Parameters.VendorId, model.VendorId);
                parameters.Add(DBConstants.Parameters.TableItems, dt_TripList?.AsTableValuedParameter("TT_ORDER_ITEMS"));

                parameters.Add(DBConstants.Parameters.OpsMode, model.OpsMode);
                parameters.Add(DBConstants.Parameters.UserId, claim.UserId);
                parameters.Add(DBConstants.OutParameters.Msg, dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                parameters.Add(DBConstants.OutParameters.MsgType, dbType: DbType.Int16, direction: ParameterDirection.Output);
                await PostAsync(DBConstants.Procedures.SaveOrderDetails, parameters, CommandType.StoredProcedure);

                var msgType = parameters.Get<Int16>(DBConstants.OutParameters.MsgType);
                var msg = parameters.Get<string>(DBConstants.OutParameters.Msg);

                return new NotifyModel(msg, msgType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<IEnumerable<OrderBaseModel>> GetOrderNoAsync()
        {
            var parameters = new DynamicParameters();
            return await GetAsync<OrderBaseModel>(DBConstants.Procedures.GetOrderNo, parameters, CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<OrderModel>> GetOrderListAsync(SearchModel model)
        {
            UserClaimModel claim = claimsAccessor.GetClaimValue();
            var parameters = new DynamicParameters();
            parameters.Add(DBConstants.Parameters.FromDate, model.FromDate);
            parameters.Add(DBConstants.Parameters.ToDate, model.ToDate);
            parameters.Add(DBConstants.Parameters.UserId, claim.UserId);
            return await GetAsync<OrderModel>(DBConstants.Procedures.GetOrderList, parameters, CommandType.StoredProcedure);

        }
        
        public async Task<IEnumerable<ItemModel>> GetOrderItemListAsync(OrderModel model)
        {
            try
            {
                UserClaimModel claim = claimsAccessor.GetClaimValue();
                var parameters = new DynamicParameters();
                parameters.Add(DBConstants.Parameters.OrderId, model.OrderId);
                return await GetAsync<ItemModel>(DBConstants.Procedures.GetOrderItemList, parameters, CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                throw;
            }
        }



    }
}
