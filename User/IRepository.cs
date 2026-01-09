namespace ZenticServer.User;

public interface IRepository
{
    public Task<Model> Get(string email);
    public Task<Model> Get(int userId);
    public Task<Model> Create(string username, string password, string email);
}