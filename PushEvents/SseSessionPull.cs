using System.Threading.Channels;

namespace ZenticServer.PushEvents;



// Пул части SSE сессий 
// В проекте несколько пулов, для оптимизации. Каждый использует свою асинхронную очередь
public class SseSessionPull
{
    private HttpResponse Response { get; set ;}
    private readonly Channel<ISessionEvent> _eventsQueue = Channel.CreateUnbounded<ISessionEvent>();
    private readonly Dictionary<int, Dictionary<int, SseSession>> _sessions = new ();
    private Task handlerEvents;

    public SseSessionPull()
    {
        handlerEvents = HandlerEvents();
    }

    public async Task SendEvent(ISessionEvent sessionEvent)
    {
        await _eventsQueue.Writer.WriteAsync(sessionEvent);
    }

    private async Task AddSession()
    {
        // Добавить сессию
    }
    
    private async Task RemoveSession()
    {
        // Удалить сессию
    }
    
    private async Task SendMessage(string data)
    {
        // Отправить сообщение клиенту
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
                    break;
                case CreateSessionEvent:
                    Console.WriteLine("CreateSessionEvent");
                    break;
                case DeleteSessionEvent:
                    Console.WriteLine("DeleteSessionEvent");
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

public record SendSessionEvent() : ISessionEvent
{
    public string Type => "send";
}
public record CreateSessionEvent() : ISessionEvent
{
    public string Type => "create";
}
public record DeleteSessionEvent() : ISessionEvent
{
    public string Type => "delete";
}
public record KeepAliveSessionEvent() : ISessionEvent
{
    public string Type => "keep_alive";
}
