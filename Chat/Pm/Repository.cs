using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ZenticServer.Chat.Pm;

public class Repository : IRepository
{
    private readonly Db.ApplicationDbContext _context;
    
    public Repository(Db.ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task Create(int firstUserId, int secondUserId)
    {
        var pmCount = await _context.Chats
            .Include(s => s.Users.Where(
                s => s.UserId == firstUserId || s.UserId == secondUserId)
            ).Where(e => 
                e.Type == Chat.Types.PersonalMessage && 
                e.Users.Count(u => u.UserId == firstUserId || u.UserId == secondUserId) == 2
            ).CountAsync();
        
        if (pmCount != 0)
            throw new Exceptions.AlreadyExists();
        _context.Chats.Add(new Chat.Model()
        {
            Type = Chat.Types.PersonalMessage, 
            Messages = new List<Message.Model>() {new Message.Model()
            {
                SenderId = firstUserId,
                Type = Message.Types.First
            }},
            ChatUsers = new List<ChatUser.Model>()
            {
                new ChatUser.Model() {Role = Chat.ChatUser.Role.Pm, UserId = firstUserId},
                new ChatUser.Model() {Role = Chat.ChatUser.Role.Pm, UserId = secondUserId}
            }
        });
        await _context.SaveChangesAsync();
        
    }
    
}