
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SalesManagement.Utilities
{
    public class DBConnector
    {
        private readonly IConfiguration config;
        public DBConnector(IConfiguration _config)
        {
            config = _config;
        }
        public IDbConnection Connection
        {
            get
            {
                var data = config.GetConnectionString("sales");
                return new SqlConnection(config.GetConnectionString("sales"));
            }
        }

        public async Task PostAsync(string sQuery, DynamicParameters parameter, CommandType type)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    await conn.ExecuteAsync(sQuery, parameter, commandType: type);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

            }

        }
        public async Task<IEnumerable<T>> GetAsync<T>(string sQuery, object parameter, CommandType type) where T : class
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    IEnumerable<T> result = await conn.QueryAsync<T>(sQuery, parameter, commandType: type);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
