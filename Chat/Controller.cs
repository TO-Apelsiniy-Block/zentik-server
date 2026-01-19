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
        Repository repository)
    {
        var chats = await repository.GetWithExtra(User.GetUserId(), offset, limit);
        return new GetChatsResponse(chats
            .ConvertAll(c => new GetChatsResponseField(
                c.Name,
                c.Chat.ChatId,
                c.Chat.Type,
                c.LastMessageText
                ))
        );

    }
    
    public record GetChatsResponse(
        [Required] [property: JsonPropertyName("chats")] List<GetChatsResponseField> Chats);
    
    public record GetChatsResponseField(
        [Required] [property: JsonPropertyName("name")] string Name,
        [Required] [property: JsonPropertyName("chat_id")] int ChatId,
        [Required] [property: JsonPropertyName("type")] string Type,
        [Required] [property: JsonPropertyName("last_message_text")] string LastMessage
        );
}
