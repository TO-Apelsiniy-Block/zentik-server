namespace ZenticServer.Chat;

public interface IRepository
{
    public Task<List<Model>> Get(int userId, int offset, int limit);
    public Task<Model> Get(int chatId);
    public Task<List<ChatListItemDto>> GetWithExtra(int userId, int offset, int limit);
    public class ChatListItemDto
    {
        public Chat.Model Chat { get; set; }
        public string Name { get; set; }
        public string LastMessageText { get; set; }
        public string LastMessageSender { get; set; }
    }
    public Task<List<User.Model>> GetUsersFromChat(int chatId, int offset, int limit);
    public Task<ChatUser.Model> GetChatUser(int chatId, int userId);
    public Task Delete(int chatId);
    public Task ClearMessages(int chatId);

}