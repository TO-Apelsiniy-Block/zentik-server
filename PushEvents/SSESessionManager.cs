using System.Collections.Concurrent;

namespace ZenticServer.PushEvents;


// Класс для управления пулом сессий SSE
public class SSESessionManager
{
    
    private static readonly Lazy<SSESessionManager> _instance = 
        new Lazy<SSESessionManager>(() => new SSESessionManager());
    
    public static SSESessionManager Instance => _instance.Value;
    
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, SSESession>> _sessionPull; 
    
    
    private SSESessionManager()
    {
        _sessionPull = new ();
    }
    
    public void AddSession(HttpContext httpContext, int chatId, int deviceId)
    {
        Console.WriteLine($"Add to session pull {chatId} {deviceId}");
        if (!_sessionPull.ContainsKey(chatId))
            _sessionPull[chatId] = new ConcurrentDictionary<int, SSESession>();
        _sessionPull[chatId][deviceId] = new SSESession(httpContext);
        httpContext.RequestAborted.Register(CloseSessionPartial(chatId, deviceId));
    }

    public async Task SendMessage(string message, EventType eventType, int chatId)
    {
        
        foreach (var session in _sessionPull[chatId].Values)
        {
            try
            {
                await session.SendAsync(message, EventTypeExtension.ToString(eventType));
            }
            catch (TaskCanceledException e)
            { }
            // Коротко - отключение пользователя до отправки, но после начала рассылки вызывает ошибку
            // Если юзер подключается во время рассылки, то может проморгать событие
        }
        
    }

    private Action CloseSessionPartial(int chatId, int deviceId)
    {
        return () => CloseSession(chatId, deviceId);
    }
    
    private void CloseSession(int chatId, int deviceId)
    {
        Console.WriteLine($"Delete from session pull {chatId} {deviceId}");
        _sessionPull[chatId].TryRemove(deviceId, out _);
        if (_sessionPull[chatId].Count == 0)
        {
            _sessionPull.TryRemove(chatId, out _);
        }
    }
}