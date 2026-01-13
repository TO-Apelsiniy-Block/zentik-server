using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenticServer.PushEvents.Events;


public record NewMessage(
    [property: JsonPropertyName("message_text")] [Required] string MessageText,
    [property: JsonPropertyName("sender_id")] [Required] int SenderId,
    [property: JsonPropertyName("sender_username")] [Required] string SenderUsername,
    [property: JsonPropertyName("chat_id")] [Required] int ChatId,
    [property: JsonPropertyName("send_time")] [Required] DateTime SendTime
);

