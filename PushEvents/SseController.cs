using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ZenticServer.PushEvents;


// Контроллер для SSE. Только принимает соединение и передает его в SSESessionManager 
[ApiController]
[Route("sse")]
public class SseController : ControllerBase
{
    [HttpGet("{chatId}")]
    public async Task GetEvents(int chatId)
    {
        Console.WriteLine("GetEvents Start");
        var ct = Response.HttpContext.RequestAborted;
        SseSessionManager.Instance.AddSession(HttpContext, chatId, Random.Shared.Next());
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(100), ct);
            }
            catch (OperationCanceledException e) {}
        }
        Console.WriteLine("GetEvents End");
    }

    [HttpGet("qwe")]
    public async Task Qwe(EventManager eventManager)
    {
        eventManager.SendEvent(new Events.NewMessage("qwe", 1, 3), 3);
    }
    
}

