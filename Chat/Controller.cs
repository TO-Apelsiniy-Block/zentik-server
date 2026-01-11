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
    public async Task ClearPm(int chatId)
    {
        // Очищение сообщений чата, без его удаления
        // Для ЛС, группы и тд имеет одинаковый интерфейс
        
    }
    
    [HttpDelete("{chatId}/delete")]
    [Authorize]
    public async Task DeletePm(int chatId)
    {
        // Полное удаление чата
        // Для ЛС, группы и тд имеет одинаковый интерфейс
    }


        
}
