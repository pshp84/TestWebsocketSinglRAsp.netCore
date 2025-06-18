using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;
using SignalRRealTimeApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRRealTimeApp.DAL.Services
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IErrorLogRepository _repository;
        public ErrorLogService(IErrorLogRepository errorLogRepository)
        {
            _repository = errorLogRepository;
        }
        public async Task<bool> LogAsync(ErrorLog error)
        {
            var res =await _repository.ErrorLogAsync(error);
            return res; 
        }

        public async Task<ErrorLogRes> GetErrorAsync(DTPagination data, FilterErrorReqDTO filter)
        {
            var res = await _repository.ErrorLogAsync(data,filter);
            return res;
        }
    }
}
