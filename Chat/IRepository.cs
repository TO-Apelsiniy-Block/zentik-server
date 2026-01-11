namespace ZenticServer.Chat;

public interface IRepository
{
    public Task<List<Model>> Get(int userId, int offset, int limit);
    public Task<List<ChatListItemDto>> GetWithExtra(int userId, int offset, int limit);
    public class ChatListItemDto
    {
        public Chat.Model Chat { get; set; }
        public string Name { get; set; }
        public string LastMessageText { get; set; }
        public string LastMessageSender { get; set; }
    }
    public Task<List<User.Model>> GetUsersFromChat(int chatId, int offset, int limit);
    public Task<List<string>> GetNames(int userId, List<int> chatsId, int offset, int limit);
    public Task Delete(int chatId);
    public Task ClearMessages(int chatId);

}