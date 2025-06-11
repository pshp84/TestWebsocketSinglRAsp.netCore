using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IRepository
{
    public interface IErrorLogRepository
    {
        Task<bool> ErrorLogAsync(ErrorLog error);
    }
}
