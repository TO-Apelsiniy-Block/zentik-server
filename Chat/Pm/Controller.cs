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
    public async Task<ActionResult> CreatePm(
        CreatePmRequest requestData,
        IRepository pmRepository)
    {
        var firstUserId = User.GetUserId();
        try
        {
            await pmRepository.Create(firstUserId, requestData.SecondUserId);

        }
        catch (Exceptions.AlreadyExists e)
        {
            return Conflict();
        }

        return Ok();
    }

    public record CreatePmRequest(
        [Required] [property: JsonPropertyName("second_user_id")] int SecondUserId);

}