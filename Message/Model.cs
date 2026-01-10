using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenticServer.Message;

public class Model : Base.Model
{ 
    [MaxLength(64)]
    public string Type;
    
    public int MessageId;
    
    public int SenderId;
    
    public int ChatId;


    public Type.TextModel? Text = null!;
    public Chat.Model Chat = null!;
    public User.Model Sender = null!;
}