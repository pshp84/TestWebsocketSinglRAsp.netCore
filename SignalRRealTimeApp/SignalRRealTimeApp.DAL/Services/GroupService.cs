using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.Services
{
    public class GroupService:IGroupService
    {
        private readonly List<GroupDTO> _groups = new(); 
        private readonly object _lock = new();
        public Task<bool> CreateGroupAsync(GroupDTO group)
        {
            lock (_lock)
            {
                if (_groups.Any(g => g.GroupName == group.GroupName))
                    return Task.FromResult(false);

                _groups.Add(group);
                return Task.FromResult(true);
            }
        }

        public Task<bool> GroupExistsAsync(string groupName)
        {
            lock (_lock)
            {
                return Task.FromResult(_groups.Any(g => g.GroupName == groupName));
            }
        }

        public Task<List<GroupDTO>> GetAllGroupsAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_groups.ToList());
            }
        }
    }
}

