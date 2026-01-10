using Microsoft.EntityFrameworkCore;

namespace ZenticServer.EmailConfirmation;

public class Repository(Db.ApplicationDbContext context) : IRepository
{
    private readonly Db.ApplicationDbContext _context = context;

    public async Task CreateOrUpdateCode(string email, int deviceId, int code)
    {
        Model emailConfirmation = new();
        emailConfirmation.Email = email;
        emailConfirmation.DeviceId = deviceId;
        emailConfirmation.Code = code;
        // Email и DeviceId составляют первичный ключ
        // Если уже поле в бд, то обновляем его
        // чтобы не плодить лишние строки
        try
        {
            _context.Add(emailConfirmation);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException == null || // TODO сделать это для всего
                e.InnerException.Message.EndsWith(
                    "duplicate key value violates unique constraint \"PK_email_confirmation\""))
                throw; // Отлавливаем только ошибку вставки из-за конфликта первичного ключа 
            await _context.EmailConfirmations
                .Where(a => a.Email == email && a.DeviceId == deviceId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(b => b.Code, code));
        }
    }

    public async Task<bool> CheckCode(string email, int deviceId, int code)
    {
        
        try
        {
            return await _context.EmailConfirmations
                .Where(a => a.Email == email && a.DeviceId == deviceId && a.Code == code)
                .ExecuteDeleteAsync() == 1;
        }
        catch (InvalidOperationException e)
        {
            return false;
        }
        return false;
    }
}