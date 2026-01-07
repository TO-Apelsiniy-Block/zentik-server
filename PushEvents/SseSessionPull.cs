using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace ZenticServer.PushEvents;



// Пул части SSE сессий 
// В проекте несколько пулов, для оптимизации. Каждый использует свою асинхронную очередь
public class SseSessionPull
{
    private readonly Channel<ISessionEvent> _eventsQueue = Channel.CreateUnbounded<ISessionEvent>();
    private readonly Dictionary<int, Dictionary<int, SseSession>> _sessions = new ();
    private Task _handlerEvents;

    public SseSessionPull()
    {
        _handlerEvents = HandlerEvents();
    }

    public void SendEvent(ISessionEvent sessionEvent)
    {
        _eventsQueue.Writer.TryWrite(sessionEvent);
    }

    private async Task AddSession(AddSessionEvent eventData)
    {
        // Добавить сессию
        if (!_sessions.ContainsKey(eventData.UserId))
        {
            _sessions[eventData.UserId] = new();
        }
        _sessions[eventData.UserId][eventData.DeviceId] = new(eventData.HttpResponse);
        eventData.HttpResponse.Headers.ContentType = "text/event-stream";
        await _sessions[eventData.UserId][eventData.DeviceId].Send(":new-connect\n\n");
        // Ставим заголовок и отправляем комментарий чтобы клиент понял что подключение прошло успешно
        eventData.HttpResponse.HttpContext.RequestAborted.Register(
            () => SendEvent(new DeleteSessionEvent(eventData.UserId, eventData.DeviceId)));

    }
    
    private async Task DeleteSession(DeleteSessionEvent eventData)
    {
        // Удалить сессию
        _sessions[eventData.UserId].Remove(eventData.DeviceId);
    }
    
    private async Task SendMessage(SendSessionEvent eventData)
    {
        // Отправить сообщение клиенту
        
        // Ищем в словаре, если нащли отправили иначе хер забили
        if (!_sessions.TryGetValue(eventData.UserId, out var userSessions))
            return;
        foreach (var session in userSessions.Values)
        {
            await session.Send(eventData.Message);
        }
    }

    private async Task SendKeepAlive()
    {
        // Рассылка keep-alive
    }
    
    private async Task HandlerEvents()
    {
        /* Обрабатывает события (не путать с событиями SSE
            (SSE - новое сообщение, изменение чата,
            здесь же - отправить сообщение, добавить/удалить сессию)
        )
        */

        await foreach (var sessionEvent in _eventsQueue.Reader.ReadAllAsync())
        {
            switch (sessionEvent)
            {
                case SendSessionEvent:
                    Console.WriteLine("SendSessionEvent");
                    await SendMessage((sessionEvent as SendSessionEvent)!);
                    break;
                case AddSessionEvent:
                    Console.WriteLine("CreateSessionEvent");
                    await AddSession((sessionEvent as AddSessionEvent)!);
                    break;
                case DeleteSessionEvent:
                    Console.WriteLine("DeleteSessionEvent");
                    await DeleteSession((sessionEvent as DeleteSessionEvent)!);
                    break;
                case KeepAliveSessionEvent:
                    Console.WriteLine("KeepAliveSessionEvent");
                    break;
                
            }
        }
    }
}

public interface ISessionEvent
{
    string Type { get; }
}

public record SendSessionEvent(
    [Required] string Message,
    [Required] int UserId
) : ISessionEvent
{
    public string Type => "send";
}


public record AddSessionEvent(
    [Required] int UserId,
    [Required] int DeviceId,
    [Required] HttpResponse HttpResponse
) : ISessionEvent
{
    public string Type => "create";
}

public record DeleteSessionEvent(
    [Required] int UserId,
    [Required] int DeviceId
) : ISessionEvent
{
    public string Type => "delete";
}

public record KeepAliveSessionEvent() : ISessionEvent
{
    public string Type => "keep_alive";
}
