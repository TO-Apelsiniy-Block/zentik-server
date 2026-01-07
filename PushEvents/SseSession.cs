namespace ZenticServer.PushEvents;

public class SseSession
{
    private HttpResponse _httpResponse;
    private CancellationToken _cancellationToken;
    public SseSession(HttpResponse httpResponse)
    {
        _httpResponse = httpResponse;
        _cancellationToken = httpResponse.HttpContext.RequestAborted;
    }

    public async Task Send(string message)
    {
        // Токен отмены не сую специально, чтобы не было необходимости отлавливать его отмену
        // В любом случае если отправим события в закрытый канал ничего не будет
        Console.WriteLine($"Write message 11111111: {message}");
        await _httpResponse.WriteAsync(message);
        Console.WriteLine($"Write message 22222222: {message}");
        await _httpResponse.BodyWriter.FlushAsync();
        Console.WriteLine($"Write message: 3333333 {message}");
    }
}