using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IServices
{
    public interface IErrorLogService
    {
        Task<bool> LogAsync(ErrorLog error);
    }
}
