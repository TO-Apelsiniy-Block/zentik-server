using System.ComponentModel.DataAnnotations;

namespace ZenticServer.Chat;

public class Model : Base.Model
{
    public int ChatId;
    [MaxLength(64)]
    public string Type; // ЛС, группа, канал

    public ICollection<Message.Model> Messages = new List<Message.Model>();
    public ICollection<Chat.ChatUser.Model> ChatUsers = new List<Chat.ChatUser.Model>();
    public ICollection<User.Model> Users = new List<User.Model>();
}