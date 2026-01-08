using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;

namespace ZenticServer.PushEvents;


// Контроллер для SSE. Только принимает соединение и передает его в SSESessionManager 
[ApiController]
[Route("sse")]
public class SseController : ControllerBase
{
    [HttpGet("{userId}")]
    [Authorize]
    public async Task GetEvents(int userId, SseSessionManager sessionManager)
    {
        Console.WriteLine("GetEvents Start");
        
        // TODO костыль для deviceId
        sessionManager.AddSession(HttpContext.Response, userId, Random.Shared.Next());
        
        // Бесконечное ожидание закрытия соединения
        var ct = Response.HttpContext.RequestAborted;
        try 
        { 
            await Task.Delay(Timeout.Infinite, ct);
        }
        catch (OperationCanceledException e) {}
        
        Console.WriteLine("GetEvents End");
    }
}

