namespace ZenticServer.Message;

public class Repository : IRepository
{
    private readonly List<Model> _messages = [];

    public Repository()
    {

        
    }
    
    public async Task<Model> CreateMessage(string text, int senderId, int chatId)
    {
        return new ();
    }

    public Task<List<Model>> GetMessages(int chatId, int offset, int limit)
    {
        if (chatId != 0)
            throw new Exceptions.NotFound();

        return Task.FromResult(_messages);
    }
}