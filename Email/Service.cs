namespace ZenticServer.Email;

public class Service : IService
{
    public Task SendCode(string email, int code)
    {
        Console.WriteLine($"SendEmailCode {email} {code}");
        return Task.CompletedTask;
    }
}