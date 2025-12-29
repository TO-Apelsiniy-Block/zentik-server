using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenticServer.Message;

public record Model(
    [Required] [property: JsonPropertyName("text")]  string Text,
    [Required] [property: JsonPropertyName("send_time")] DateTime SendTime,
    [Required] [property: JsonPropertyName("message_id")] int MessageId,
    [Required] [property: JsonPropertyName("sender_id")] int SenderId,
    [Required] [property: JsonPropertyName("chat_id")] int ChatId);