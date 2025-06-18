using Microsoft.AspNetCore.SignalR;
using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Concurrent;

namespace SignalRRealTimeApp.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IChatService _chatservice;
        private readonly ILogger<NotificationHub> _logger;
        private readonly IChatTrackingService _tracking;
        private readonly ILiveLogService _liveLogService;
        private readonly IGroupService _groupService;
        private static ConcurrentDictionary<string, string> _userGroups = new();

        public NotificationHub(IChatService chatService, ILogger<NotificationHub> logger, IChatTrackingService trackingService,ILiveLogService liveLogService, IGroupService groupService)
        {
            _chatservice = chatService;
            _logger = logger;
            _tracking = trackingService;
            _liveLogService = liveLogService;
            _groupService = groupService;   
        }
        public async Task SendMessage(string user, string message, string fileName = null, byte[] fileBytes = null)
       {
            try
            {
                string fileUrl = null;

                if (fileBytes != null && !string.IsNullOrEmpty(fileName))
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    await File.WriteAllBytesAsync(filePath, fileBytes);
                    fileUrl = $"/uploads/{uniqueFileName}";
                    fileName = uniqueFileName;
                }
                var chatMessage = new ChatMessageDTO
                {
                    User = user,
                    Message = message,
                    Timestamp = DateTime.Now,
                    FileName = fileName,
                    FileContent = fileBytes
                };

                _logger.LogInformation($"Message from {user}: {message}, File: {fileName}");

                await _chatservice.AddMessage(chatMessage);
                await Clients.All.SendAsync("ReceiveMessage", user, message, fileUrl);
                await SendLog($"Message sent by {user} - {(string.IsNullOrEmpty(message) ? "[File]" : message)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessage failed");
                throw;
            }
        }


        public override async Task OnConnectedAsync()
        {
            try
            {
                _logger.LogInformation($"Client connected: {Context.ConnectionId}");
                await SendLog($"Client connected: {Context.ConnectionId}");
                await _tracking.AddConnectionAsync(Context.ConnectionId);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public  override async Task OnDisconnectedAsync(Exception? exception)
        {

            try
            {
                _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
                await SendLog($"Client disconnected: {Context.ConnectionId}");
                await _tracking.RemoveConnectionAsync(Context.ConnectionId);
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex) { throw; }

        }


        public async Task JoinGroup(string groupName)
        {
            _userGroups[Context.ConnectionId] = groupName;
            if (!await _groupService.GroupExistsAsync(groupName))
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Group '{groupName}' does not exist.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var added = await _tracking.AddToGroupAsync(groupName, Context.ConnectionId);

            if (added)
            {
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined {groupName}");
                await SendLog($"Client {Context.ConnectionId} joined group '{groupName}'");
            }
        }


        public async Task LeaveGroup(string groupName)
        {
            try
            {
                _userGroups.TryRemove(Context.ConnectionId, out _);
                await _tracking.RemoveFromGroupAsync(groupName, Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} left {groupName}");
                await SendLog($"Client {Context.ConnectionId} left group '{groupName}'");
            }
            catch (Exception ex) { throw; }
        }

        public async Task SendToUser(string connectionId, string message)
        {
            try
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Private", message);
                await SendLog($"Private message sent to {connectionId}");
            }
            catch (Exception ex) 
            { 
                throw; 
            }
        }

        public async Task SendLog(string log)
        {
           
            await _liveLogService.AddLogAsync(log);
            await Clients.All.SendAsync("ReceiveLog", log);
        }
        public async Task<bool> CreateGroup(string groupName)
        {
            var group = new GroupDTO
            {
                GroupName = groupName,
                CreatedBy = Context.ConnectionId,
                CreatedAt = DateTime.Now
            };

            var created = await _groupService.CreateGroupAsync(group);

            if (created)
            {
                await SendLog($"Group '{groupName}' created by {Context.ConnectionId}");
            }

            return created;
        }

        public async Task SendMessageToGroup(string user, string message)
        {
            if (_userGroups.TryGetValue(Context.ConnectionId, out var groupName))
            {
                await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "You are not in any group.");
            }
        }

    }
}
