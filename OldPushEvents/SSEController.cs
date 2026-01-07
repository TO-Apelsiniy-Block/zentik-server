using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using ZenticServer.Auth;

namespace ZenticServer.OldPushEvents;


// Контроллер для SSE. Только принимает соединение и передает его в SSESessionManager 
[ApiController]
[Route("sseOLDOLDOLDOLDODLDLDLDLDLDOLD_OLD")]
public class SSEController : ControllerBase
{
    [HttpGet("{chatId}")]
    [Authorize]
    public async Task GetEvents(int chatId)
    {
        Console.WriteLine("GetEvents Start");
        var ct = Response.HttpContext.RequestAborted;
        SSESessionManager.Instance.AddSession(HttpContext, User.GetUserId(), Random.Shared.Next());
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
}

