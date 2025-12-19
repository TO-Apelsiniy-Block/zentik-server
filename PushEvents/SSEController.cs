using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ZenticServer.PushEvents;


[ApiController]
[Route("api/[controller]")]
public class SSEController : ControllerBase
{
    private readonly Channel<string> _messagesQueue = Channel.CreateUnbounded<string>();

    
    [HttpGet("{userId}")]
    public async Task GetEvents(int userId)
    {
        Console.WriteLine("GetEvents Start");
        var ct = Response.HttpContext.RequestAborted;
        var writer = _messagesQueue.Writer;
        Response.Headers.ContentType = "text/event-stream";
        var keepAlive = _KeepAlive(ct);
        var senderMessages = _SenderMessages(ct);
        
        await writer.WriteAsync("data: started\n\n", ct);
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await writer.WriteAsync("data: message\n\n", ct);
                await Task.Delay(TimeSpan.FromSeconds(2), ct);
            }
            catch (OperationCanceledException e) {}
        }
        Console.WriteLine("GetEvents End");
    }

    private async Task _SenderMessages(CancellationToken ct)
    {
        Console.WriteLine("SenderMessages Start");
        await foreach (var message in _messagesQueue.Reader.ReadAllAsync(ct))
        {
            try
            { 
                await Response.WriteAsync(message, ct); 
                await Response.Body.FlushAsync(ct);
            }
            catch (OperationCanceledException e) {}
        }

        Console.WriteLine("SenderMessages End");
    }
    
    private async Task _KeepAlive(CancellationToken ct)
    {
        Console.WriteLine("KeepAlive Start");
        
        var writer = _messagesQueue.Writer;
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2), ct);
                await writer.WriteAsync(":keep-alive\n\n", ct);
            } 
            catch (OperationCanceledException e) {}
        }

        Console.WriteLine("KeepAlive End");
    }
}

