using Microsoft.EntityFrameworkCore;
using TrustWaveCarca.Data;

namespace TrustWaveCarca.Services
{
    public class ChatMessageService
    {
        private readonly ApplicationDbContext _context;

        public ChatMessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageToDatabase(ChatMessage chatMessage)
        {
            // Check if the message with the same messageId already exists in the database
            var existingMessage = await _context.ChatMessages
                .FirstOrDefaultAsync(m => m.MessageId == chatMessage.MessageId);

            if (existingMessage == null)
            {
                // If the message doesn't exist, save it to the database
                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<ChatMessage>> GetMessages(string userId, string targetUserId)
        {
            return await _context.ChatMessages
                .Where(m => (m.SenderId == userId && m.ReceiverId == targetUserId) ||
                            (m.SenderId == targetUserId && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<ChatMessage> GetMessageById(string messageId)
        {
            // Logic to fetch message by messageId from your database
            var message = await _context.ChatMessages
                .FirstOrDefaultAsync(m => m.MessageId == messageId);

            return message;
        }
    }

}
