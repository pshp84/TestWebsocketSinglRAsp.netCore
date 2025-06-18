using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DTO;


namespace SignalRRealTimeApp.DAL.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
        _chatRepository = chatRepository;
        }
        public  async Task<ChatMessageDTO> AddMessage(ChatMessageDTO message)
        {
            var res= await _chatRepository.AddChatMessage(message);  
            return res; 
        }

        public async Task<List<ChatMessageDTO>> GetAllMessages()
        {
           var res=await _chatRepository.GetAllChatMessages();
            return res; 
        }

        public async Task<bool> ClearAllMessages()
        {
            var res=await _chatRepository.RemoveAllMessage();
            return res;  
        }
    }
}
