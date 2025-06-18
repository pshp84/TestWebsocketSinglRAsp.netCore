using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IRepository
{
    public interface ILiveLogService
    {
        Task<bool>AddLogAsync(string log);
        Task<List<string>> GetLogs();
    }
}
