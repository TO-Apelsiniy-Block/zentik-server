using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.PushEvents;

using System.Threading.Channels;


// Класс для удобной работы с сессией SSE
public class SSESession
{
    private HttpResponse Response { get; set ;}
    private readonly Channel<string> _eventsQueue = Channel.CreateUnbounded<string>();
    private Task _sender;
    private Task _keeper;
    
    public CancellationToken SessionClosedToken { get; private set; }
    
    public SSESession(HttpContext httpContext)
    {
        Response = httpContext.Response;
        SessionClosedToken = httpContext.RequestAborted;
        _sender = SenderMessagesAsync();
        _keeper = KeeperAlive(httpContext.RequestAborted);
    }
    
    public async Task SendAsync(string data)
    {
        await _eventsQueue.Writer.WriteAsync($"data: {data}\n\n", SessionClosedToken);
    }
    
    public async Task SendAsync(string data, string event_)
    {
        await _eventsQueue.Writer.WriteAsync($"event: {event_}\ndata: {data}\n\n", SessionClosedToken);
    }

    private async Task SenderMessagesAsync()
    {
        Console.WriteLine("SenderMessages Start");
        var ct = Response.HttpContext.RequestAborted;
        Response.Headers.ContentType = "text/event-stream";
        
        try
        {
            // Для того чтобы клиент понял что будет SSE сразу отправляем комментарий,
            // так как с ним сразу же отправятся заголовки
            await Response.WriteAsync(":start\n\n", ct);
            await Response.Body.FlushAsync(ct);
            
            await foreach (var message in _eventsQueue.Reader.ReadAllAsync(ct))
            {
                await Response.WriteAsync(message, ct);
                await Response.Body.FlushAsync(ct);
            }
        }
        catch (OperationCanceledException e) { }
        Console.WriteLine("SenderMessages End");
    }
    
    private async Task KeeperAlive(CancellationToken ct)
    {
        Console.WriteLine("KeepAlive Start");
        
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(15), ct);
                await _eventsQueue.Writer.WriteAsync(":keep-alive\n\n", ct);
            } 
            catch (OperationCanceledException e) {}
        }

        Console.WriteLine("KeepAlive End");
    }
}
