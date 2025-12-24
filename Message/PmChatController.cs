using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Message;

// Контроллер для личных сообщений (ЛС): создание чатов, удаление
// Сюда выносятся эндпоинты имеющие разные интерфейсы для разных типов чатов
[ApiController]
[Route("chat/pm")]
public class PmChatController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task CreatePm(CreatePmRequest requestData)
    {
        
    }

    public record CreatePmRequest(
        [Required] [property: JsonPropertyName("second_user_id")] int SecondUserId);

}