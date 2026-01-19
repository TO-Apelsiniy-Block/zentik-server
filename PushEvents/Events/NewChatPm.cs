using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenticServer.PushEvents.Events;

public record NewChatPm
(
    [property: JsonPropertyName("last_message_text")] [Required] string LastMessageText,
    [property: JsonPropertyName("chat_name")] [Required] string ChatName,
    [property: JsonPropertyName("chat_id")] [Required] int ChatId
);