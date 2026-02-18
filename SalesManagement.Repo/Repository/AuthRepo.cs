using Dapper;
using Microsoft.Extensions.Configuration;
using SalesManagement.Domain.Constants;
using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.User;
using SalesManagement.Repo.Interface;
using SalesManagement.Utilities;
using System.Data;

namespace SalesManagement.Repo.Repository
{
    public class AuthRepo : DBConnector, IAuthRepo
    {
        public AuthRepo(IConfiguration configuration) : base(configuration) { }


        public async Task<NotifyModel> GetValidateLoginAsync(LoginModel model)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(DBConstants.Parameters.UserName, model.UserName);
                parameters.Add(DBConstants.Parameters.Password, model.Password);
                parameters.Add(DBConstants.OutParameters.Msg, dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                parameters.Add(DBConstants.OutParameters.MsgType, dbType: DbType.Int16, direction: ParameterDirection.Output, size: 5215585);
                parameters.Add(DBConstants.OutParameters.RetId, dbType: DbType.Int64, direction: ParameterDirection.Output, size: 5215585);
                await PostAsync(DBConstants.Procedures.ValidateLogin, parameters, CommandType.StoredProcedure);
                var msgType = parameters.Get<Int16>(DBConstants.OutParameters.MsgType);
                var msg = parameters.Get<string>(DBConstants.OutParameters.Msg);
                var retrn = parameters.Get<Int64>(DBConstants.OutParameters.RetId);
                return new NotifyModel(msg, msgType, retrn.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
