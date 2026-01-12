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
        PushEvents.EventManager eventManager,
        Chat.IRepository chatRepository)
    {
        try
        {
            await repository.CreateMessage(bodyData.Text, 
                User.GetUserId(), bodyData.ChatId);
        }
        catch (Exceptions.NotFound e)
        {
            return Results.NotFound("Chat not found");
        }
        foreach (var chatMember in await chatRepository.GetUsersFromChat(
                     bodyData.ChatId, 0, Int32.MaxValue))
        {
            eventManager.SendMessage(
                new PushEvents.Events.NewMessage(bodyData.Text, User.GetUserId(), bodyData.ChatId), 
                chatMember.UserId);
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
        int offset, int limit, int chatId, 
        IRepository repository, 
        Chat.IRepository chatRepository)
    {
        try
        {
            var chatUser = await chatRepository.GetChatUser(chatId, User.GetUserId()); 
        }
        catch (Exceptions.NotFound e)
        {
            return Forbid(); // TODO сделать нормальные ошибки
        }
        try
        {
            var messages = await repository.GetMessages(chatId, offset, limit);
            return  new GetMessagesFromChatResponse(
                messages.ConvertAll(m => new GetMessagesFromChatResponseField(
                    m.Text,
                    m.MessageId,
                    m.SendTime,
                    m.SenderId,
                    m.SenderUsername
                    ))); 
        }
        catch (Exceptions.NotFound e)
        {
            return NotFound("Chat not found");
        }
    }
    
    public record GetMessagesFromChatResponse(
        [Required] [property: JsonPropertyName("messages")] List<GetMessagesFromChatResponseField> Chats);
    public record GetMessagesFromChatResponseField(
        [Required] [property: JsonPropertyName("text")] string Text,
        [Required] [property: JsonPropertyName("message_id")] int MessageId,
        [Required] [property: JsonPropertyName("send_time")] DateTime SendTime,
        [Required] [property: JsonPropertyName("sender_id")] int SenderId,
        [Required] [property: JsonPropertyName("sender_username")] string SenderUsername
        );
}


    


