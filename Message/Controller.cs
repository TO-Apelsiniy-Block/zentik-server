using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZenticServer.Auth;

namespace ZenticServer.Message;

// Хуйня, что так много синглтонов


// Контроллер для работы с сообщениями: отправка, изменение, удаление
[ApiController]
[Route("message")]
public class Controller : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IResult> WriteMessage(
        NewMessageData bodyData, 
        IRepository repository,
        PushEvents.EventManager eventManager)
    {
        try
        {
            await repository.CreateMessage(bodyData.Text, 
                User.GetUserId(), bodyData.ChatId);
            eventManager.SendMessage(
                new PushEvents.Events.NewMessage(bodyData.Text, User.GetUserId(), bodyData.ChatId), 
                bodyData.ChatId);
        }
        catch (Exceptions.NotFound e)
        {
            return Results.NotFound("Chat not found");
        }

        return Results.Ok();
    }
    
    public record NewMessageData(
        [Required] [property: JsonPropertyName("text")] string Text,
        [Required] [property: JsonPropertyName("chat_id")] int ChatId
    );
    
    
    [HttpGet("{chatId}")]
    [Authorize]
    public async Task<ActionResult<GetMessagesFromChatResponse>> GetMessagesFromChat(
        int offset, int limit, int chatId, IRepository repository)
    {
        return new GetMessagesFromChatResponse(
            (await repository.GetMessages(chatId, offset, limit)).ConvertAll(model =>
                new GetMessagesFromChatResponseField(model.Text, model.CreatedAt, model.ChatId, model.MessageId)));
    }
    
    public record GetMessagesFromChatResponse(
        [Required] [property: JsonPropertyName("chats")] List<GetMessagesFromChatResponseField> Chats);
    public record GetMessagesFromChatResponseField(
        [Required] [property: JsonPropertyName("text")] string Text,
        [Required] [property: JsonPropertyName("send_time")] DateTime SendTime,
        [Required] [property: JsonPropertyName("chat_id")] int ChatId,
        [Required] [property: JsonPropertyName("message_id")] int MessageId);
}


    


