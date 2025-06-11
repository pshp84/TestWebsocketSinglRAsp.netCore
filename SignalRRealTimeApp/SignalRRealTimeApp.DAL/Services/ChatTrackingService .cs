using SignalRRealTimeApp.DAL.IServices;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.Services
{
    public class ChatTrackingService: IChatTrackingService
    {
        private readonly ConcurrentDictionary<string, byte> _connections = new();
        private readonly ConcurrentDictionary<string, HashSet<string>> _groups = new();
        public Task<bool> AddConnectionAsync(string connectionId)
        {
            var result = _connections.TryAdd(connectionId, 0);
            return Task.FromResult(result);
        }

        public Task<bool> RemoveConnectionAsync(string connectionId)
        {
            var result = _connections.TryRemove(connectionId, out _);
            return Task.FromResult(result);
        }

        public Task<bool> AddToGroupAsync(string groupName, string connectionId)
        {
            var group = _groups.GetOrAdd(groupName, _ => new HashSet<string>());

            lock (group)
            {
                return Task.FromResult(group.Add(connectionId));
            }
        }

        public Task<bool> RemoveFromGroupAsync(string groupName, string connectionId)
        {
            if (_groups.TryGetValue(groupName, out var group))
            {
                lock (group)
                {
                    return Task.FromResult(group.Remove(connectionId));
                }
            }
            return Task.FromResult(false);
        }

        public Task<Dictionary<string, int>> GetGroupSummaryAsync()
        {
            var result = _groups.ToDictionary(g => g.Key, g => g.Value.Count);
            return Task.FromResult(result);
        }

        public Task<int> GetTotalConnectionsAsync()
        {
            return Task.FromResult(_connections.Count);
        }
    }

}
