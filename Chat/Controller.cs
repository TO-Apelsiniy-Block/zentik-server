using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.Auth;

namespace ZenticServer.Chat;

// Контроллер для работы с чатами (ЛС, группы, каналы) 
// Здесь то, что имеет одинаковый интерфейс в независимости от типа чата 
[ApiController]
[Route("chat")]
public class Controller : ControllerBase
{
    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<GetChatsResponse>> GetChats(
        int offset, int limit, 
        IRepository repository)
    {
        var chats = await repository.GetWithExtra(User.GetUserId(), offset, limit);
        return new GetChatsResponse(chats
            .ConvertAll(c => new GetChatsResponseField(
                "1",
                c.Chat.ChatId,
                c.Name,
                c.LastMessageText,
                c.LastMessageSender
                ))
        );

    }
    
    public record GetChatsResponse(
        [Required] [property: JsonPropertyName("chats")] List<GetChatsResponseField> Chats);
    
    public record GetChatsResponseField(
        [Required] [property: JsonPropertyName("name")] string Name,
        [Required] [property: JsonPropertyName("chat_id")] int ChatId,
        [Required] [property: JsonPropertyName("type")] string Type,
        [Required] [property: JsonPropertyName("last_message_text")] string LastMessage,
        [Required] [property: JsonPropertyName("last_message_sender")] string LastMessageSender
        );
    
    
    [HttpDelete("{chatId}/clear")]
    [Authorize]
    public async Task<ActionResult> ClearPm(
        int chatId, IRepository repository)
    {
        // Очищение сообщений чата, без его удаления
        // Для ЛС, группы и тд имеет одинаковый интерфейс
        try
        {
            var chatUser = await repository.GetChatUser(chatId, User.GetUserId());
            var chat = await repository.Get(chatId);
            if (!AccessVerify.ClearChat(chat.Type, chatUser.Role)) 
                return Forbid();
            await repository.ClearMessages(chatId);
            return Ok();
        }
        catch (Exceptions.NotFound e)
        {
            return NotFound();
        }
    }
    
    [HttpDelete("{chatId}/delete")]
    [Authorize]
    public async Task<ActionResult> DeletePm(
        int chatId, IRepository repository)
    {
        // Полное удаление чата
        // Для ЛС, группы и тд имеет одинаковый интерфейс
        try
        {
            var chatUser = await repository.GetChatUser(chatId, User.GetUserId());
            var chat = await repository.Get(chatId);
            if (AccessVerify.DeleteChat(chat.Type, chatUser.Role))
            {
                await repository.Delete(chatId);
                return Ok();
            }
            return Forbid();
        }
        catch (Exceptions.NotFound e)
        {
            return NotFound();
        }
    }


        
}
