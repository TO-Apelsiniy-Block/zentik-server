namespace ZenticServer.EmailConfirmation;

public class Repository(Db.ApplicationDbContext context) : IRepository
{
    private readonly Db.ApplicationDbContext _context = context;

    public async Task CreateCode(string email, int deviceId)
    {
        
    }

    public async Task GetCode(string email, int deviceId)
    {
        
    }
}