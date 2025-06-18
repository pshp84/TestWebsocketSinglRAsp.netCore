using Dapper;
using Microsoft.Extensions.Configuration;
using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DTO;
using SignalRRealTimeApp.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRRealTimeApp.DAL.Repository
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly IConfiguration _configuration;
        public string connectionString = string.Empty;
        public ErrorLogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = ConfigurationExtensions.GetConnectionString(_configuration, "ConStr");
        }
        public async Task<bool> ErrorLogAsync(ErrorLog error)
        {
            using (var Connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameter = new DynamicParameters();
                dynamicParameter.Add("@Message", error.Message);
                dynamicParameter.Add("@StackTrace", error.StackTrace);
                dynamicParameter.Add("@Controller", error.Controller);
                dynamicParameter.Add("@CreatedAt", DateTime.UtcNow);
                dynamicParameter.Add("@Url", error.Url);
                var res = await Connection.QueryAsync<bool>("dbo.[usp_insertErrorLog]", dynamicParameter, commandType: CommandType.StoredProcedure);
                if (res.Count() == 0) { return false; }
                return true;
            }
        }

        public async Task<ErrorLogRes> ErrorLogAsync(DTPagination data, FilterErrorReqDTO filter)
        {
            using (var connection=new SqlConnection(connectionString))
            {
                    string SortColumn = null;
                    string SortOrder = null;
                    if (data.order != null)
                    {
                        SortOrder = data.order[0].dir;
                        SortColumn = data.columns[data.order[0].column].data;
                    }
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@OffsetValue", data.start);
                    dynamicParameters.Add("@PageSize", data.length);
                    dynamicParameters.Add("@Search", data.search.value);
                    dynamicParameters.Add("@SortColumn", SortColumn);
                    dynamicParameters.Add("@SortOrder", SortOrder);
                    dynamicParameters.Add("@Status", filter.Status);
                    var reader = await connection.QueryMultipleAsync("dbo.[usp_GetErrorLogs]", dynamicParameters, commandType: CommandType.StoredProcedure);
                    var TotalRow = reader.Read<int>().FirstOrDefault();
                    var FilterRowCount = reader.Read<int>().FirstOrDefault();
                    var resdata = reader.Read<ErrorLogEnity>().ToList();
                    ErrorLogRes rdata = new ErrorLogRes()
                    {
                        recordsTotal = TotalRow,
                        recordsFiltered = FilterRowCount,
                        data = resdata  
                    };
                    return rdata;   
                
                

            }
        }
    }
}
