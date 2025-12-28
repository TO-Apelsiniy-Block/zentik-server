using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.PushEvents;
using ZenticServer.PushEvents.Events;
using Microsoft.AspNetCore.Authorization;
using ZenticServer.Auth;

namespace ZenticServer.Message;

// Хуйня, что так много синглтонов


// Контроллер для работы с сообщениями: отправка, изменение, удаление, получение
[ApiController]
[Route("message")]
public class MessageController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IResult> WriteMessage(
        NewMessageData newMessageData, 
        IMessageRepository messageRepository)
    {
        var userId = User.GetUserId();
        await messageRepository.CreateMessage(newMessageData.Text, 
            User.GetUserId(), newMessageData.ChatId);
        await EventManager.Instance.SendEvent(
            new NewMessageEvent(newMessageData.Text, userId, newMessageData.ChatId), 
            EventType.NewMessage, newMessageData.ChatId);
        return Results.Ok();
    }
    
    [HttpGet("{chatId}")]
    [Authorize]
    public async Task<ActionResult<GetMessagesFromChatResponse>> GetMessagesFromChat(
        int offset, int limit, int chatId, IMessageRepository messageRepository)
    {
        return new GetMessagesFromChatResponse(
            (await messageRepository.GetMessages(chatId, offset, limit)).ConvertAll(model =>
                new GetMessagesFromChatResponseField(model.Text, model.SendTime, model.ChatId, model.MessageId)));

    }
}

public record NewMessageData(
    [Required] [property: JsonPropertyName("text")] string Text,
    [Required] [property: JsonPropertyName("chat_id")] int ChatId
    );
    

public record GetMessagesFromChatResponse(
    [Required] [property: JsonPropertyName("chats")] List<GetMessagesFromChatResponseField> Chats);
public record GetMessagesFromChatResponseField(
    [Required] [property: JsonPropertyName("text")] string Text,
    [Required] [property: JsonPropertyName("send_time")] DateTime SendTime,
    [Required] [property: JsonPropertyName("chat_id")] int ChatId,
    [Required] [property: JsonPropertyName("message_id")] int MessageId);
