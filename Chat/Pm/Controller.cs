using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.Auth;

namespace ZenticServer.Chat.Pm;

// Контроллер для личных сообщений (ЛС): создание чатов, удаление
// Сюда выносятся контроллеры имеющие разные интерфейсы для разных типов чатов
[ApiController]
[Route("chat/pm")]
public class Controller : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreatePmResponse>> CreatePm(
        CreatePmRequest requestData,
        IRepository pmRepository)
    {
        var firstUserId = User.GetUserId();
        if (requestData.SecondUserId == firstUserId) return Conflict();
        try
        {
            await pmRepository.Create(firstUserId, requestData.SecondUserId);
        }
        catch (Exceptions.AlreadyExists e)
        {
            return Conflict();
        }
        return Ok(new CreatePmResponse(1, "1"));
    }

    public record CreatePmRequest(
        [Required] [property: JsonPropertyName("second_user_id")] int SecondUserId);
    public record CreatePmResponse(
        [Required] [property: JsonPropertyName("chat_id")] int ChatId,
        [Required] [property: JsonPropertyName("chat_name")] string ChatName
        // [Required] [property: JsonPropertyName("last_message_text")] string LastMessageText
        // [Required] [property: JsonPropertyName("last_message_sender")] string LastMessageSender
        );

}