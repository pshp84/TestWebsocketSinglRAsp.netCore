using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IServices
{
    public interface IChatTrackingService
    {
        Task<bool> AddConnectionAsync(string connectionId);
        Task<bool> RemoveConnectionAsync(string connectionId);

        Task<bool> AddToGroupAsync(string groupName, string connectionId);
        Task<bool> RemoveFromGroupAsync(string groupName, string connectionId);

        Task<Dictionary<string, int>> GetGroupSummaryAsync();
        Task<int> GetTotalConnectionsAsync();
    }
}
