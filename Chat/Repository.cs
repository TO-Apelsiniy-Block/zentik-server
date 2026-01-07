namespace ZenticServer.Chat;

public class Repository : IRepository 
{
    public async Task<List<Model>> Get(int userId, int offset, int limit)
    {
        var res = new List<Model>();
        res.Add(new Model("Chat 1", "Pm"));
        return res;
    }

    public async Task<List<User.Model>> GetAllUsers(int chatId, int offset, int limit)
    {
        var res = new List<User.Model>();
        res.Add(new (1, "said", "said@q.q"));
        res.Add(new (2, "said2", "said2@q.q"));
        return res;
    }
    
}


