namespace ZenticServer.PushEvents;

public class SseSession
{
    private readonly HttpResponse _httpResponse;
    private CancellationToken _cancellationToken;
    public SseSession(HttpResponse httpResponse)
    {
        _httpResponse = httpResponse;
        _cancellationToken = httpResponse.HttpContext.RequestAborted;
    }

    public async Task Send(string message)
    {
        // Токен отмены не сую специально, чтобы не было необходимости отлавливать его отмену
        // В любом случае если отправим события в закрытый канал ничего страшного не будет
        
        await _httpResponse.WriteAsync(message);
        if (!message.EndsWith("\n\n"))
            await _httpResponse.WriteAsync("\n\n");
        await _httpResponse.BodyWriter.FlushAsync();
    }

    public async Task Close()
    {
        _httpResponse.HttpContext.Abort();
    }
}