using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.PushEvents;
using ZenticServer.PushEvents.Events;
using Microsoft.AspNetCore.Authorization;
using ZenticServer.Auth;

namespace ZenticServer.Message;


// Контроллер для работы с сообщениями: отправка, изменение, удаление, получение
[ApiController]
[Route("message")]
public class MessageController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IResult> WriteMessage(NewMessageData newMessageData)
    {
        var userId = User.GetUserId();
        await EventManager.Instance.SendEvent(
            new NewMessageEvent(newMessageData.Message, userId, newMessageData.ChatId), 
            EventType.NewMessage, 
            newMessageData.ChatId
        );
        return Results.Ok();
    }
    
    [HttpGet]
    [Authorize]
    public async Task GetMessagesFromChat(int offset, int limit)
    {
        
    }
}

public record NewMessageData(
    [Required] [property: JsonPropertyName("message")] string Message,
    [Required] [property: JsonPropertyName("chat_id")] int ChatId
    );