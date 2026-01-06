using System.Collections.Concurrent;

namespace ZenticServer.PushEvents;


// Класс для управления пулами сессий SSE
public class SseSessionManager
{
    
    private static readonly Lazy<SseSessionManager> _instance = 
        new Lazy<SseSessionManager>(() => new SseSessionManager());
    
    public static SseSessionManager Instance => _instance.Value;
    
    private readonly List<SseSessionPull> _channels; 
    
    private SseSessionManager()
    {
        _channels = new (5);
        for (int i=0; i<5; i++)
        {
            _channels.Add(new SseSessionPull());
        }
    }
    
    public void AddSession(HttpContext httpContext, int chatId, int deviceId)
    {
    }

    public async Task SendMessage(string message, Types types, int chatId)
    {
        await _channels[0].SendEvent(new SendSessionEvent());
    }
}