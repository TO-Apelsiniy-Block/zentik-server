namespace ZenticServer.Chat.ChatUser;

public class Model : Base.Model
{
    public int UserId;
    public int ChatId;
    public string Role;

    public User.Model User = null!;
    public Chat.Model Chat = null!;
}