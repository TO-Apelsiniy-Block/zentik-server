namespace ZenticServer.Chat;

public interface IRepository
{
    public Task<List<Model>> Get(int userId, int offset, int limit);
    public Task<List<User.Model>> GetUsersFromChat(int chatId, int offset, int limit);
    
}