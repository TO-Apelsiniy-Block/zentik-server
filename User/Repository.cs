namespace ZenticServer.User;

public class Repository : IRepository
{
    public async Task<Model> Get(string email)
    {
        if (email != "said@q.q")
            throw new Exceptions.NotFound();
        return new Model(1, "said", "said@q.q");
    }

    public async Task<Model> Get(int userId)
    {
        if (userId != 1)
            throw new Exceptions.NotFound();
        return new Model(1, "said", "said@q.q");
    }
    

    public async Task<Model> Create(string username, string email)
    {
        return new Model(2, username, email);
    }
}