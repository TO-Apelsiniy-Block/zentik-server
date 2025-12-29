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
        { // А если будет добавлено одновременно две новые сессии и одна из них сотрет другую? 
            _sessionPull[chatId] = new ConcurrentDictionary<int, SSESession>();
        }

        _sessionPull[chatId][deviceId] = new SSESession(httpContext);
        httpContext.RequestAborted.Register(CloseSessionPartial(chatId, deviceId));
    }

    public async Task SendMessage(string message, EventType eventType, int chatId)
    {
        // Возвращает true, если сообщение отправлено хотя бы на одно из устройств 
        foreach (var session in _sessionPull[chatId].Values)
        {
            try
            {
                await session.SendAsync(message, EventTypeExtension.ToString(eventType));
            }
            catch (TaskCanceledException e)
            { }
            // Отключение пользователя до отправки, но после начала рассылки вызывает ошибку
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
        { // А если в этот момент произойдет подключение?
            _sessionPull.TryRemove(chatId, out _);
        }
    }
}