namespace ZenticServer.Chat;

public class Repository : IRepository 
{
    public async Task<List<Model>> Get(int userId, int offset, int limit)
    {
        var res = new List<Model>();
        return res;
    }

    public async Task<List<User.Model>> GetUsersFromChat(int chatId, int offset, int limit)
    {
        var res = new List<User.Model>();
        return res;
    }
    
}


