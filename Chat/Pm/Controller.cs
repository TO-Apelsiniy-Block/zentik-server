using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenticServer.Auth;
using ZenticServer.PushEvents.Events;

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
        PushEvents.EventManager eventManager,
        Repository pmRepository)
    {
        var firstUserId = User.GetUserId();
        if (requestData.SecondUserId == firstUserId) return BadRequest();
        Repository.CreatePmDto qwe;
        try
        {
            qwe = await pmRepository.Create(firstUserId, requestData.SecondUserId);
        }
        catch (Exceptions.AlreadyExists e)
        { // TODO
            return Conflict();
        }
        eventManager.NewChatPm(
            new NewChatPm("Новый чат", qwe.Name, qwe.ChatId), 
            requestData.SecondUserId);
        return Ok(new CreatePmResponse(qwe.ChatId, qwe.Name));
    }

    public record CreatePmRequest(
        [Required] [property: JsonPropertyName("second_user_id")] int SecondUserId);
    public record CreatePmResponse(
        [Required] [property: JsonPropertyName("chat_id")] int ChatId,
        [Required] [property: JsonPropertyName("chat_name")] string ChatName
        // [Required] [property: JsonPropertyName("last_message_text")] string LastMessageText
        // [Required] [property: JsonPropertyName("last_message_sender")] string LastMessageSender
        );
            
            
    [HttpPost("qwe")]
    [Authorize]
    public async Task<ActionResult<CreatePmResponse>> GetChatIdPm(
        CreatePmRequest requestData,
        Repository pmRepository)
    { // TODO Временный на минимум
        var firstUserId = User.GetUserId();

        var ewq = await pmRepository.GetIdByUsersIds(firstUserId, requestData.SecondUserId);
        return Ok(new CreatePmResponse(ewq.ChatId, ewq.Name));
    }
    

}