using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IRepository
{
    public interface IChatRepository
    {
        Task<ChatMessageDTO> AddChatMessage(ChatMessageDTO data);
        Task<List<ChatMessageDTO>> GetAllChatMessages();
        Task<bool> RemoveAllMessage();
    }
}
