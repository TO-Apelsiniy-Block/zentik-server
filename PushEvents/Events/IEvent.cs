namespace ZenticServer.PushEvents.Events;

public interface IEvent
{
    Types Type { get; }
}