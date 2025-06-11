using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Mvc;
using SignalRRealTimeApp.DAL.IServices;

namespace SignalRRealTimeApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IChatTrackingService _trackingService;
        public ChatController(IChatService chatService, IChatTrackingService trackingService)
        {
            _chatService = chatService;
            _trackingService = trackingService; 
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChatView()
        {
            return View();
        }
        public async Task<IActionResult> GetMessages()
        {
            var messages= _chatService.GetAllMessages();
            if (messages == null) { return null; }     
            return Json(messages.Result);
        }
        [HttpPost]
        public async Task<IActionResult> ClearMessages()
        {
            bool result = await _chatService.ClearAllMessages();
            return Ok(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetConnectionStats()
        {
            var groupData = await _trackingService.GetGroupSummaryAsync();
            var connectionCount =await _trackingService.GetTotalConnectionsAsync();

            return Json(new
            {
                totalConnections = connectionCount,
                groups = groupData
            });
        }
    }
}
