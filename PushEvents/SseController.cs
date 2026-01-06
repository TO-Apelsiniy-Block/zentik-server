using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ZenticServer.PushEvents;


// Контроллер для SSE. Только принимает соединение и передает его в SSESessionManager 
[ApiController]
[Route("sse")]
public class SseController : ControllerBase
{
    [HttpGet("{chatId}")]
    public async Task GetEvents(int chatId, SseSessionManager sessionManager)
    {
        Console.WriteLine("GetEvents Start");
        
        sessionManager.AddSession(HttpContext, chatId, Random.Shared.Next());
        
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

