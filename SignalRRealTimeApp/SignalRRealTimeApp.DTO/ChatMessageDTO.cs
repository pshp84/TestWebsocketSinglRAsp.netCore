

namespace SignalRRealTimeApp.DTO
{
    public class ChatMessageDTO
    {
        public string? User { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }

        // Optional file properties
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileUrl => !string.IsNullOrEmpty(FileName)
            ? $"/uploads/{FileName}" // assuming you're saving original name
            : null;
    }
}
