namespace ZenticServer.Chat.Pm;

public interface IRepository
{
    public Task Create(int firstUserId, int secondUserId);
}