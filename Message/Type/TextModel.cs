using System.ComponentModel.DataAnnotations;

namespace ZenticServer.Message.Type;

public class TextModel
{
    public int MessageId;
    [MaxLength(4096)]
    public string Text;

    public Message.Model Message = null!;
}