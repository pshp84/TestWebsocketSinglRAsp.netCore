using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IServices
{
    public interface IGroupService
    {
        Task<bool> CreateGroupAsync(GroupDTO group);
        Task<bool> GroupExistsAsync(string groupName);
        Task<List<GroupDTO>> GetAllGroupsAsync();
    }
}
