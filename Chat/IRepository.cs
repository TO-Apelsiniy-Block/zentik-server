namespace ZenticServer.Chat;

public interface IRepository
{
    public Task<List<Model>> Get(int userId, int offset, int limit);
}