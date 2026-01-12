using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZenticServer.Auth;

namespace ZenticServer.PushEvents;


// Контроллер для SSE. Только принимает соединение и передает его в SSESessionManager 
[ApiController]
[Route("sse")]
public class SseController : ControllerBase
{
    [HttpGet("")]
    [Authorize]
    public async Task GetEvents(int deviceId, SseSessionManager sessionManager)
    {
        sessionManager.AddSession(HttpContext.Response, User.GetUserId(), deviceId);
        
        // Бесконечное ожидание закрытия соединения
        var ct = Response.HttpContext.RequestAborted;
        try 
        { 
            await Task.Delay(Timeout.Infinite, ct);
        }
        catch (OperationCanceledException e) {}
    }
}

