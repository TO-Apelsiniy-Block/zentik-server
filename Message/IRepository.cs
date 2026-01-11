using System.Collections;

namespace ZenticServer.Message;

public interface IRepository
{
    public Task<Model> CreateMessage(string text, int senderId, int chatId);
    public Task<List<MesssageListItemDto>> GetMessages(int chatId, int offset, int limit);

    public class MesssageListItemDto
    {
        public string Text { get; set; }
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public DateTime SendTime { get; set; }
    }
}