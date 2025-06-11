using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;


namespace SignalRRealTimeApp.DAL.Services
{
    public class ChatService : IChatService
    {
        private readonly List<ChatMessageDTO> _messages = new();
        public  async Task<bool> AddMessage(ChatMessageDTO message)
        {
            if(message == null) return false;
            _messages.Add(message);
            return true;
        }

        public async Task<List<ChatMessageDTO>> GetAllMessages()
        {
            return  _messages.ToList();
        }

        public async Task<bool> ClearAllMessages()
        {
            if (_messages.Count() > 0)
            {
                _messages.Clear();
                return !_messages.Any();

            }         
            else {return false; }   
        }
    }
}
