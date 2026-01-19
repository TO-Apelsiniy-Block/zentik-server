using System.Collections;
using Microsoft.EntityFrameworkCore;
using ZenticServer.Message.Type;

namespace ZenticServer.Message;

public class Repository
{
    private readonly Db.ApplicationDbContext _context;
    
    public Repository(Db.ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Model> CreateMessage(string text, int senderId, int chatId)
    {
        var newMessage = new Model()
        {
            ChatId = chatId,
            SenderId = senderId,
            Type = Types.Text,
            Text = new TextModel()
            {
                Text = text
            }
        };
        await _context.Messages.AddAsync(newMessage);
        await _context.SaveChangesAsync();
        return newMessage;
    }

    public async Task<List<MesssageListItemDto>> GetMessages(
        int chatId, int offset, int limit)
    {
        return await _context.Messages
            .Where(c => c.ChatId == chatId && c.Type != Types.First)
            .Select(m => new MesssageListItemDto
                {
                Text = m.Type == Types.Text ? m.Text.Text : "Unknown message",
                MessageId = m.MessageId,
                SendTime = m.CreatedAt,
                SenderId = m.SenderId,
                SenderUsername = m.Sender.Username
            }).ToListAsync();
    }
    public class MesssageListItemDto
    {
        public string Text { get; set; }
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public DateTime SendTime { get; set; }
    }
    
}