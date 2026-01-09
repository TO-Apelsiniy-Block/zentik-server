using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Chat;

// Контроллер для работы с чатами (ЛС, группы, каналы) 
// Здесь то, что имеет одинаковый интерфейс в независимости от типа чата 
[ApiController]
[Route("chat")]
public class Controller : ControllerBase
{
    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<GetChatsResponse>> GetChats(int offset, int limit)
    {
        return new GetChatsResponse(null);
    }
    
    public record GetChatsResponse(
        [Required] [property: JsonPropertyName("chats")] List<GetChatsResponseField> Chats);
    
    public record GetChatsResponseField(
        [Required] [property: JsonPropertyName("last_message")] string LastMessage,
        [Required] [property: JsonPropertyName("name")] string Name,
        [Required] [property: JsonPropertyName("chat_id")] int ChatId);
    
    
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
