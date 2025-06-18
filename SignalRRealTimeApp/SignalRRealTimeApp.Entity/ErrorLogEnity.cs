using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.Entity
{

    public class ErrorLogRes : DTCommonRes
    {
        public List<ErrorLogEnity> data { get; set; }
    }
    public class ErrorLogEnity
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? Url { get; set; }
        public string? InnerException { get; set; }
        public DateTime Createdon { get; set; }
        public string Status { get; set; }
        
    }

    public class FilterErrorReqDTO
    {
        public int? Status { get; set; }
    }
}
