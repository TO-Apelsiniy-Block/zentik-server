namespace ZenticServer.Email;

public interface IService
{
    public Task SendCode(string email, int code);
}