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
    
    public async Task SendMessage(Events.NewMessage eventData, int userId)
    { 
        // Событие отправляется именно юзеру 
        _sessionManager.Send(JsonSerializer.Serialize(eventData), EventTypes.NewMessage, userId);
    }
}