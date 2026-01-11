using System.Collections.Frozen;
using Microsoft.EntityFrameworkCore;

namespace ZenticServer.Chat;

public class Repository : IRepository 
{
    private readonly Db.ApplicationDbContext _context;
    
    public Repository(Db.ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Model>> Get(int userId, int offset, int limit)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Chats)
                .Where(u => u.UserId == userId)
                .FirstAsync();
            return user.Chats.ToList();
        }
        catch (InvalidOperationException e)
        {
            throw new Exceptions.NotFound();
        }

    }


    public async Task<List<IRepository.ChatListItemDto>> GetWithExtra(
        int userId, int offset, int limit)
    {
        // Получение чатов пользователя вместе с:
        // последним сообщение, отправителем последнего сообщения
        
        try
        {
            /*
            var chats = await _context.Chats
                .Include(c => c.Users
                    .Where(u => u.UserId == userId))
                .Include(c => c.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(1)
                )
                .ThenInclude(m => m.Sender)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .ToListAsync(); */

            var chats = await _context.Chats
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .Select(c => new IRepository.ChatListItemDto
                {
                    Chat = c,
                    Name = c.Type == Types.PersonalMessage
                            ? c.Users
                            .Where(u => u.UserId != userId)
                            .Select(u => u.Username)
                            .First()
                                : c.Type == Types.Group
                            ? "group"
                                : "Unknown chat",
                    LastMessageText = c.Messages
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => m.Type == Message.Types.Text
                        ? m.Text.Text
                            : m.Type ==  Message.Types.First
                        ? "Hello world!"
                            : "Unknown message"
                        )
                        .First(),
                    LastMessageSender = c.Messages
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => m.Sender.Username)
                        .First()
                })
                .ToListAsync();

            return chats;
        }
        catch (InvalidOperationException e)
        {
            throw new Exceptions.NotFound();
        }
    }


    public async Task<List<string>> GetNames(int userId, List<int> chatsId, int offset, int limit)
    {
        return (await _context.Chats
            .Select(c => new {Chat = c.Type})
            .ToListAsync()
        ).ConvertAll(a => a.Chat);
    }
        
    
    
    public async Task<List<User.Model>> GetUsersFromChat(int chatId, int offset, int limit)
    {
        try
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .Where(c => c.ChatId == chatId)
                .FirstAsync();
            return chat.Users.ToList();
        }
        catch (InvalidOperationException e)
        {
            throw new Exceptions.NotFound();
        }
    }

    
    public async Task Delete(int chatId)
    { // Удаление именно чата
        await _context.Chats
            .Where(c => c.ChatId == chatId)
            .ExecuteDeleteAsync();
    }

    
    public async Task ClearMessages(int chatId)
    { // Удаление именно сообщений
        // TODO сделать тип сообщение "AfterClear" для его вставки, вместо сохранения первого
        await _context.Messages
            .Where(c => c.ChatId == chatId && c.Type != Message.Types.First)
            .ExecuteDeleteAsync();
    }
}


