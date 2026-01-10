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
        try
        {
            var user = await _context.Users.Where(b => b.UserId == userId).FirstAsync();
            return user;
        }
        catch (InvalidOperationException e)
        {
            throw new Exceptions.NotFound();
        }
    }
    

    public async Task<Model> Create(string username, string password, string email)
    {
        Model user = new()
        {
            Username = username,
            Password = password,
            Email = email
        };
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // TODO перенести коммит в другое место
        }
        catch (DbUpdateException e)
        {
            throw new Exceptions.AlreadyExists();
        }
        return user;
    }
}