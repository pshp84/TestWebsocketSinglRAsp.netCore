using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;
using System.Security.Claims;

namespace SignalRRealTimeApp.Models
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IErrorLogService _errorLogService;
        private readonly IConfiguration _configuration;
        private readonly int _dberror;
        private readonly int _emailerror;
        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration, IErrorLogService errorLogServices)
        {
            _next = next;
            _errorLogService = errorLogServices;
            _configuration = configuration;
            _dberror = Convert.ToInt32(_configuration["ErrorLog:Database"]);
            _emailerror = Convert.ToInt32(_configuration["ErrorLog:Email"]);
        }

        public object User { get; private set; }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.Path.Value == "/Home/Error")
                {
                    context.Response.StatusCode = 500;
                }
                
                await _next(context);
                if (context.Response.StatusCode != 200)
                {
                    var response = context.Response;
                    if (response.StatusCode == 404)
                    {
                        response.Redirect("/Home/Error", true);
                    }
                }
            }
            catch (Exception ex)
            {
                
                var response = context.Response;
                response.ContentType = "application/json";
                var hostname = response.HttpContext.Request.Host.Value;
                ErrorLog errorLog = new ErrorLog();
                errorLog.Message = ex.Message; 
                errorLog.StackTrace = ex.StackTrace;
                errorLog.Url = hostname + response.HttpContext.Request.Path.Value;
                errorLog.Action = "UnhandledException";
                errorLog.InnerException = Convert.ToString(ex?.InnerException);
                _errorLogService.LogAsync(errorLog).Wait();
                

                response.Redirect("/Home/Error", true);
            }
        }
    }
}
