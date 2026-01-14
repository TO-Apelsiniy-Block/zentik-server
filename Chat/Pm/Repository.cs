using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ZenticServer.Exceptions;

namespace ZenticServer.Chat.Pm;

public class Repository : IRepository
{
    private readonly Db.ApplicationDbContext _context;
    
    public Repository(Db.ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<IRepository.CreatePmDto> Create(int firstUserId, int secondUserId)
    {
        var pmCount = await _context.Chats
            .Include(s => s.Users.Where(
                u => u.UserId == firstUserId || u.UserId == secondUserId)
            ).Where(e => 
                e.Type == Chat.Types.PersonalMessage && 
                e.Users.All(u => u.UserId == firstUserId || u.UserId == secondUserId)
            ).CountAsync();
        
        if (pmCount != 0) // TODO А если юзера нету?
            throw new Exceptions.AlreadyExists();
        var newChat = _context.Chats.Add(new Chat.Model()
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
        }).Entity;
        await _context.SaveChangesAsync();

        return new()
        {
            ChatId = newChat.ChatId,
            Name = "asd",
            FirstMessageSender = "awd",
            FirstMessageText = "awd"
        };
    }
}