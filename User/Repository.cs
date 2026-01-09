using Microsoft.EntityFrameworkCore;

namespace ZenticServer.User;

public class Repository : IRepository
{
    private readonly Db.ApplicationDbContext _context;
    public Repository(Db.ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<Model> Get(string email)
    {
        try
        {
            var user = await _context.Users.Where(b => b.Email == email).FirstAsync();
            return user;
        }
        catch (InvalidOperationException e)
        {
            throw new Exceptions.NotFound();
        }
    }

    public async Task<Model> Get(int userId)
    {
        if (userId != 1)
            throw new Exceptions.NotFound();
        return new Model(1, "said", "said@q.q");
    }
    

    public async Task<Model> Create(string username, string password, string email)
    {
        User.Model user = new();
        user.Username = username;
        user.Password = password;
        user.Email = email;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        // TODO перенести коммит в другое место
        return user;
    }
}