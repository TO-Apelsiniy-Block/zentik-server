namespace ZenticServer.PushEvents;


// Класс для управления пулом сессий SSE
public class SSESessionManager
{
    
    private static readonly Lazy<SSESessionManager> _instance = 
        new Lazy<SSESessionManager>(() => new SSESessionManager());
    
    public static SSESessionManager Instance => _instance.Value;
    
    private SSESessionManager()
    {
        _sessionPull = new ();
    }
    
    private readonly Dictionary<int, SortedDictionary<int, SSESession>> _sessionPull; 

    public void AddSession(HttpContext httpContext, int chatId, int deviceId)
    {
        Console.WriteLine($"Add to session pull {chatId} {deviceId}");
        if (!_sessionPull.ContainsKey(chatId))
            _sessionPull[chatId] = new SortedDictionary<int, SSESession>();
        _sessionPull[chatId][deviceId] = new SSESession(httpContext);
        httpContext.RequestAborted.Register(CloseSessionPartial(chatId, deviceId));
    }

    public async Task SendMessage(string message, EventType eventType, int chatId)
    {
        foreach (var session in _sessionPull[chatId].Values)
        {
            await session.SendAsync(message, EventTypeExtension.ToString(eventType));
            
        }
    }

    private Action CloseSessionPartial(int chatId, int deviceId)
    {
        return () => CloseSession(chatId, deviceId);
    }
    
    private void CloseSession(int chatId, int deviceId)
    {
        Console.WriteLine($"Delete from session pull {chatId} {deviceId}");
        _sessionPull[chatId].Remove(deviceId);
        if (_sessionPull[chatId].Count == 0)
        {
            _sessionPull.Remove(chatId);
        }
    }
}