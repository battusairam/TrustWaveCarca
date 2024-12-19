namespace TrustWaveCarca.Data
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow.Add(new TimeSpan(5, 30, 0));
        public bool IsRead { get; set; }

    }

}
