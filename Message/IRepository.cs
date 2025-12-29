namespace ZenticServer.Message;

public interface IRepository
{
    public Task<Model> CreateMessage(string text, int senderId, int chatId);
    public Task<List<Model>> GetMessages(int chatId, int offset, int limit);
}