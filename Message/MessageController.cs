using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.PushEvents;
using ZenticServer.PushEvents.Events;

namespace ZenticServer.Message;


// Контроллер для работы с сообщениями: отправка, изменение, удаление
[ApiController]
[Route("api/message")]
public class MessageController
{
    [HttpPost]
    public async Task WriteMessage(NewMessageData newMessageData)
    {
        await EventManager.Instance.SendEvent(
            new NewMessageEvent(newMessageData.Message, 0, newMessageData.ChatId), 
            EventType.NewMessage, 
            newMessageData.ChatId
        );
    }
}

public record NewMessageData(
    [Required] [property: JsonPropertyName("message")] string Message,
    [Required] [property: JsonPropertyName("chat_id")] int ChatId
    );