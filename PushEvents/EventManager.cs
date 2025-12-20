using System.Text.Json;

namespace ZenticServer.PushEvents;


/*
  Класс для обработки событий. Работает с SSESessionManager и БД
*/ 
public class EventManager
{
    private static readonly Lazy<EventManager> _instance = 
        new Lazy<EventManager>(() => new EventManager());

    private readonly SSESessionManager _sessionManager;
    public static EventManager Instance => _instance.Value;

    private EventManager()
    {
        _sessionManager = SSESessionManager.Instance;
    }
    
    public async Task SendEvent<TEventData>(TEventData eventData, EventType eventType, int chatId)
    { // А если сессии нема?
        await _sessionManager.SendMessage(JsonSerializer.Serialize<TEventData>(eventData), eventType, chatId);
    }
}