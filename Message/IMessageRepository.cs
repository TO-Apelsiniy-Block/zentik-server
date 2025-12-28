using System.Security.Cryptography.X509Certificates;

namespace ZenticServer.Message;

public interface IMessageRepository
{
    public Task<MessageModel> CreateMessage(string text, int senderId, int chatId);
    public Task<List<MessageModel>> GetMessages(int chatId, int offset, int limit);
}