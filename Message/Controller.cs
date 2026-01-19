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
    public async Task<ActionResult> WriteMessage(
        NewMessageData bodyData, 
        Repository repository,
        PushEvents.EventManager eventManager,
        Chat.Repository chatRepository)
    {
        try
        {
            var chatUser = await chatRepository.GetChatUser(bodyData.ChatId, User.GetUserId()); 
        }
        catch (Exceptions.NotFound e)
        {
            return Forbid(); // TODO сделать нормальные ошибки
        }
        var newMessage = await repository.CreateMessage(
            bodyData.Text, 
            User.GetUserId(), 
            bodyData.ChatId);

        var chatMembers = await chatRepository.GetUsersFromChat(
            bodyData.ChatId,
            0,
            Int32.MaxValue);
        foreach (var chatMember in chatMembers)
        {
            if (chatMember.UserId == User.GetUserId())
            {
                continue;
            }
            eventManager.SendMessage(
                new PushEvents.Events.NewMessage(
                    bodyData.Text,
                    User.GetUserId(),
                    newMessage.Sender.Username,
                    newMessage.ChatId,
                    newMessage.CreatedAt),
                chatMember.UserId);
        }

        return Ok();
    }
    
    public record NewMessageData(
        [Required] [property: JsonPropertyName("text")] string Text,
        [Required] [property: JsonPropertyName("chat_id")] int ChatId
    );
    
    
    [HttpGet("{chatId}")]
    [Authorize]
    [ProducesResponseType(typeof(GetMessagesFromChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GetMessagesFromChatResponse>> GetMessagesFromChat(
        int offset, int limit, int chatId, 
        Repository repository, 
        Chat.Repository chatRepository)
    {
        try
        {
            var chatUser = await chatRepository.GetChatUser(chatId, User.GetUserId()); 
        }
        catch (Exceptions.NotFound e)
        {
            return Forbid(); // TODO сделать нормальные ошибки
        }
        var messages = await repository.GetMessages(chatId, offset, limit);
        var result = new GetMessagesFromChatResponse(
            messages.ConvertAll(m => new GetMessagesFromChatResponseField(
                m.Text, // m.MessageId, 
                m.SendTime,
                m.SenderId, m.SenderUsername)), User.GetUserId());
        return Ok(result);
    }
    
    public record GetMessagesFromChatResponse(
        [Required] [property: JsonPropertyName("messages")] List<GetMessagesFromChatResponseField> Chats,
        [Required] [property: JsonPropertyName("user_id")] int UserId
        );
    public record GetMessagesFromChatResponseField(
        [Required] [property: JsonPropertyName("text")] string Text,
        [Required] [property: JsonPropertyName("send_time")] DateTime SendTime,
        [Required] [property: JsonPropertyName("sender_id")] int SenderId,
        [Required] [property: JsonPropertyName("sender_username")] string SenderUsername
        );
}


    


