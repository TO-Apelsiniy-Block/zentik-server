using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenticServer.Message;

public class Model : Base.Model
{ 
    [MaxLength(4096)]
    public string Text;
    
    public int MessageId;
    
    public int SenderId;
    
    public int ChatId;


    public Chat.Model Chat = null!;
    public User.Model Sender = null!;
}