using SignalRRealTimeApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.IServices
{
    public interface IChatService
    {
       Task<bool>AddMessage(ChatMessageDTO message);
       Task <List<ChatMessageDTO>> GetAllMessages();
        Task<bool> ClearAllMessages();
    }
}
