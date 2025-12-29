namespace ZenticServer.Chat;

public class Repository : IRepository 
{
    public async Task<List<Model>> Get(int userId, int offset, int limit)
    {
        var res = new List<Model>();
        res.Add(new Model("Chat 1", "Pm"));
        return res;

    }
    
}


