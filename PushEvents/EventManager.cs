using System.Text.Json;

namespace ZenticServer.PushEvents;


/*
  Класс для обработки событий. Работает с SSESessionManager и БД
  Singleton
*/ 
public class EventManager
{
    private readonly SseSessionManager _sessionManager;

    public EventManager(SseSessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }
    
    public async Task SendEvent(Events.IEvent eventData, int chatId)
    { // А если сессии нема?
        await _sessionManager.SendMessage(JsonSerializer.Serialize(eventData), eventData.Type, chatId);
    }
}