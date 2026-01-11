using System.Collections;

namespace ZenticServer.Message;

public class Repository : IRepository
{
    private readonly Db.ApplicationDbContext _context;
    
    public Repository(Db.ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Model> CreateMessage(string text, int senderId, int chatId)
    {
        return new ();
    }

    public async Task<List<Model>> GetMessages(int chatId, int offset, int limit)
    {
        return new List<Model>();
    }
}