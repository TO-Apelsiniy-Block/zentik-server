using System.Collections.Concurrent;

namespace ZenticServer.Message;

public class Repository : IRepository
{
    private readonly List<Model> _messages = [];

    public Repository()
    {
        _messages.Add(new Model(
            "hi", 
            DateTime.Now,
            Guid.NewGuid().GetHashCode(), 
            12, 
            0));
        _messages.Add(new Model(
            "HELLO", 
            DateTime.Now,
            Guid.NewGuid().GetHashCode(), 
            12002, 
            0));
        _messages.Add(new Model(
            "SOSI HUI", 
            DateTime.Now,
            Guid.NewGuid().GetHashCode(), 
            12002, 
            0));
        _messages.Add(new Model(
            "SLAVA ROSSII", 
            DateTime.Now,
            Guid.NewGuid().GetHashCode(), 
            12, 
            0));
        
    }
    
    public async Task<Model> CreateMessage(string text, int senderId, int chatId)
    {
        var newMessage = new Model(
            text, 
            DateTime.Now,
            Guid.NewGuid().GetHashCode(), 
            senderId, 
            chatId);
        return newMessage;
    }

    public Task<List<Model>> GetMessages(int chatId, int offset, int limit)
    {
        if (chatId != 0)
            throw new Exceptions.NotFound();

        return Task.FromResult(_messages);
    }
}