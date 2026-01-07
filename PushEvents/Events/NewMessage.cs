using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenticServer.PushEvents.Events;


public record NewMessage(
    [property: JsonPropertyName("message_text")] [Required] string MessageText,
    [property: JsonPropertyName("sender_id")] [Required] int SenderId,
    [property: JsonPropertyName("chat_id")] [Required] int ChatId
    // Время отправки, айди отправителя и тд
) : IEvent;

