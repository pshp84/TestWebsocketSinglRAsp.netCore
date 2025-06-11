using Dapper;
using Microsoft.Extensions.Configuration;
using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SignalRRealTimeApp.DAL.Repository
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        public string connectionstring = string.Empty;
        private readonly IConfiguration configuration;
        public ErrorLogRepository(IConfiguration configuration) 
        {
            connectionstring = ConfigurationExtensions.GetConnectionString(configuration, "ConnectionStrings");
        } 
        public async Task<bool> ErrorLogAsync(ErrorLog error)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Message", error.Message);
                dynamicParameters.Add("@StackTrace", error.StackTrace);
                dynamicParameters.Add("@Controller", error.Controller);
                dynamicParameters.Add("@Action", error.Action);
                dynamicParameters.Add("@CreatedAt", DateTime.UtcNow);
                dynamicParameters.Add("@Url",error.Url );
                dynamicParameters.Add("@InnerException",error.InnerException);

                var affectedRows = await connection.ExecuteAsync(
                "dbo.usp_insertErrorLog",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                return affectedRows > 0;
            }
        }
    }
}
