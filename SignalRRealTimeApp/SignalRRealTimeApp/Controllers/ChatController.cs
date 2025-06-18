using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Mvc;
using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DAL.Services;

namespace SignalRRealTimeApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IChatTrackingService _trackingService;
        private readonly ILiveLogService _logService;
        private readonly IGroupService _groupService;
        public ChatController(IChatService chatService, IChatTrackingService trackingService,ILiveLogService liveLogService,IGroupService groupService)
        {
            _chatService = chatService;
            _trackingService = trackingService; 
            _logService = liveLogService;
            _groupService= groupService;    
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
            var messages = await _chatService.GetAllMessages();

            var result = messages.Select(m => new {
                user = m.User,
                message = m.Message,
                fileUrl = string.IsNullOrEmpty(m.FileName) ? null : $"/uploads/{m.FileName}"
            });

            return Ok(result);
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
            var connectionCount = await _trackingService.GetTotalConnectionsAsync();
            return Json(new
            {
                totalConnections = connectionCount,
                groups = groupData
            });
        }

        public async Task<IActionResult> GetLogs()
        {
            var logs = await _logService.GetLogs();
            return Json(logs);
        }
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Json(groups);
        }

        public async Task<IActionResult> GroupChat()
        {
            return View();
        }
    }
}
