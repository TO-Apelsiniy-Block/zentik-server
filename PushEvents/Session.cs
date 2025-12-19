using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.PushEvents;

using System.Threading.Channels;

public class Session
{
    private HttpResponse Response { get; set ;}
    private readonly Channel<string> _eventsQueue = Channel.CreateUnbounded<string>();
    
    public Session(HttpResponse response)
    {
        Response = response;
        SenderMessagesAsync();
        Console.WriteLine("Start Session");
    }
    
    public async Task SendAsync(string data)
    {
        await _eventsQueue.Writer.WriteAsync($"data: {data}");
    }
    
    public async Task SendAsync(string data, string event_)
    {
        await _eventsQueue.Writer.WriteAsync($"event: {event_}\ndata: {data}");
    }

    private async Task SenderMessagesAsync()
    {
        var ct = Response.HttpContext.RequestAborted;
        try
        {
            await foreach (var message in _eventsQueue.Reader.ReadAllAsync(ct))
            {
                await Response.WriteAsync(message, ct);
                await Response.Body.FlushAsync(ct);
            }
        }
        catch (OperationCanceledException e) { }
    }
}
