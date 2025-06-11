using Microsoft.AspNetCore.SignalR;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;

namespace SignalRRealTimeApp.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IChatService _chatservice;
        private readonly ILogger<NotificationHub> _logger;
        private readonly IChatTrackingService _tracking;

        public NotificationHub(IChatService chatService, ILogger<NotificationHub> logger, IChatTrackingService trackingService)
        {
            _chatservice = chatService;
            _logger = logger;
            _tracking = trackingService;
        }
        public async Task SendMessage(string user, string message)
        {
            try
            {
                var chatMessage = new ChatMessageDTO
                {
                    User = user,
                    Message = message,
                    Timestamp = DateTime.Now
                };
                _logger.LogInformation($"Message from {user}: {message}");
                await _chatservice.AddMessage(chatMessage);
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex) { throw; }

        }
        public override Task OnConnectedAsync()
        {
            try
            {
                _logger.LogInformation($"Client connected: {Context.ConnectionId}");
                _tracking.AddConnectionAsync(Context.ConnectionId);
                return base.OnConnectedAsync();
            }
            catch (Exception ex) { throw; }

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {

            try
            {
                _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
                _tracking.RemoveConnectionAsync(Context.ConnectionId);
                return base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex) { throw; }

        }


        public async Task JoinGroup(string groupName)
        {
            try
            {
                await _tracking.AddToGroupAsync(groupName, Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined {groupName}");
            }
            catch (Exception ex) { throw; }
        }

        public async Task LeaveGroup(string groupName)
        {
            try
            {
                await _tracking.RemoveFromGroupAsync(groupName, Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} left {groupName}");
            }
            catch (Exception ex) { throw; }
        }

        public async Task SendToUser(string connectionId, string message)
        {
            try
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Private", message);
            }
            catch (Exception ex) 
            { 
                throw; 
            }
        }



    }
}
