using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.PushEvents;
using ZenticServer.PushEvents.Events;
using Microsoft.AspNetCore.Authorization;
using ZenticServer.Auth;

namespace ZenticServer.Message;


// Контроллер для работы с сообщениями: отправка, изменение, удаление
[ApiController]
[Route("api/message")]
public class MessageController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task WriteMessage(NewMessageData newMessageData)
    {
        var userId = User.GetUserId();
        await EventManager.Instance.SendEvent(
            new NewMessageEvent(newMessageData.Message, userId, newMessageData.ChatId), 
            EventType.NewMessage, 
            newMessageData.ChatId
        );
    }
}

public record NewMessageData(
    [Required] [property: JsonPropertyName("message")] string Message,
    [Required] [property: JsonPropertyName("chat_id")] int ChatId
    );