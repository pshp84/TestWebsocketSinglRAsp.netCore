using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;
using SignalRRealTimeApp.Entity;
using SignalRRealTimeApp.Models;

namespace SignalRRealTimeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IErrorLogService _errorLogService;

        public HomeController(ILogger<HomeController> logger,IErrorLogService errorLogService)
        {
            _logger = logger;
            _errorLogService = errorLogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChatPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult>GetErrorLog()
        {
            return View();
        }
        public async Task<JsonResult> GetAllErrorLog(DTPagination data, FilterErrorReqDTO filter)
        {
            var res=await _errorLogService.GetErrorAsync(data, filter); 
            return Json(res);
        }

        public async Task<IActionResult> Logs()
        {
            return View();  
        }
    }
}
