using Dapper;
using Microsoft.Extensions.Configuration;
using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DTO;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRRealTimeApp.DAL.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly IConfiguration _configuration; 
        public string connectionString=string.Empty;
        public ChatRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = ConfigurationExtensions.GetConnectionString(_configuration, "ConStr");
        }

        public async Task<ChatMessageDTO> AddChatMessage(ChatMessageDTO data)
        {
            using (var Connection= new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameter=new DynamicParameters();
                dynamicParameter.Add("@User", data.User);
                dynamicParameter.Add("@Message", data.Message);
                dynamicParameter.Add("@Timestamp", data.Timestamp);
                dynamicParameter.Add("@FileName", data.FileName);
                dynamicParameter.Add("@FileUrl", data.FileUrl);
                var res = await Connection.QueryAsync<ChatMessageDTO>("dbo.AddChatMessage", dynamicParameter,commandType: CommandType.StoredProcedure);
                if (res.Count() == 0) { return null; }  
                return res.FirstOrDefault();            
            }

        }

        public async Task<List<ChatMessageDTO>> GetAllChatMessages()
        {
            using (var Connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameter = new DynamicParameters();
                var res = await Connection.QueryAsync<ChatMessageDTO>("dbo.GetAllChatMessages", dynamicParameter, commandType: CommandType.StoredProcedure);
                return res?.ToList() ?? new List<ChatMessageDTO>();
            }
        }

        public async Task<bool> RemoveAllMessage()
        {
            using (var Connection = new SqlConnection(connectionString))
            {
                DynamicParameters dynamicParameter = new DynamicParameters();
                var res = await Connection.QueryAsync<bool>("dbo.ClearAllChatMessages", dynamicParameter, commandType: CommandType.StoredProcedure);
                if (res.Count() == 0) { return false; }
                return true;
            }
        }
    }
}
