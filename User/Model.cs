using System.ComponentModel.DataAnnotations;

namespace ZenticServer.User;

public class Model : Base.Model
{
    public int UserId  { get; set; }
    
    [MaxLength(64)]
    public string Username  { get; set; }
    
    [MaxLength(64)]
    public string Email { get; set; }
    
    [MaxLength(256)]
    public string Password { get; set; }
    
    public ICollection<Chat.ChatUser.Model> ChatUsers = new List<Chat.ChatUser.Model>();
    public ICollection<Chat.Model> Chats = new List<Chat.Model>();
    public ICollection<Message.Model> Messages = new List<Message.Model>();
    
}