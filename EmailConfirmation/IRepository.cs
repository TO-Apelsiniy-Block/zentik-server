namespace ZenticServer.EmailConfirmation;

public interface IRepository
{
    public Task CreateOrUpdateCode(string email, int deviceId, int code);

    public Task<bool> CheckCode(string email, int deviceId, int code);
    public Task DeleteCode(string email, int deviceId);
}