using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace ZenticServer.PushEvents;



// Пул части SSE сессий 
// В проекте несколько пулов, для оптимизации. Каждый использует свою асинхронную очередь
public class SseSessionPull
{
    private readonly Channel<ISessionEvent> _eventsQueue = Channel.CreateUnbounded<ISessionEvent>();
    private readonly Dictionary<int, Dictionary<int, SseSession>> _sessions = new ();
    private readonly Task _handlerEvents;
    

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
        if (!_sessions.TryGetValue(eventData.UserId, out var userSessions))
            _sessions[eventData.UserId] = new();
        if (_sessions[eventData.UserId].TryGetValue(eventData.DeviceId, out var oldSession))
        { // Если одно устройство установило соединение два раза, то закрываем оба
            Console.WriteLine("Close start");
            eventData.HttpResponse.HttpContext.Abort();
            Console.WriteLine(eventData.HttpResponse.HttpContext.RequestAborted.IsCancellationRequested);
            
            await _sessions[eventData.UserId][eventData.DeviceId].Close();
            Console.WriteLine("Close end");
            
            return; 
        }
        _sessions[eventData.UserId][eventData.DeviceId] = new(eventData.HttpResponse);
        
        eventData.HttpResponse.Headers.ContentType = "text/event-stream";
        await _sessions[eventData.UserId][eventData.DeviceId].Send(":new-connect");
        // Ставим заголовок и отправляем комментарий чтобы клиент понял что подключение прошло успешно
        
        eventData.HttpResponse.HttpContext.RequestAborted.Register(
            () => SendEvent(new DeleteSessionEvent(eventData.UserId, eventData.DeviceId)));
        // Прокидываем callback для обработки закрытия

    }
    
    private async Task DeleteSession(DeleteSessionEvent eventData)
    {
        // Удалить сессию
        _sessions[eventData.UserId].Remove(eventData.DeviceId);
    }
    
    private async Task SendMessage(SendSessionEvent eventData)
    {
        // Отправить сообщение клиенту
        
        // Ищем в словаре, если нашли отправили иначе хер забили
        if (!_sessions.TryGetValue(eventData.UserId, out var userSessions))
            return;
        foreach (var session in userSessions.Values)
        {
            await session.Send(eventData.Message);
        }
    }

    private async Task SendKeepAlive(KeepAliveSessionEvent eventData)
    {
        // Рассылка keep-alive
        foreach (var userSessions in _sessions.Values)
        {
            foreach (var session in userSessions.Values)
            {
                await session.Send(":keep-alive\n\n");
            }
        }
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
                    await SendKeepAlive((sessionEvent as KeepAliveSessionEvent)!);
                    break;
                
            }
        }
    }
}

public interface ISessionEvent
{
}

public record SendSessionEvent(
    [Required] string Message,
    [Required] int UserId
) : ISessionEvent;


public record AddSessionEvent(
    [Required] int UserId,
    [Required] int DeviceId,
    [Required] HttpResponse HttpResponse
) : ISessionEvent;

public record DeleteSessionEvent(
    [Required] int UserId,
    [Required] int DeviceId
) : ISessionEvent;

public record KeepAliveSessionEvent() : ISessionEvent;
