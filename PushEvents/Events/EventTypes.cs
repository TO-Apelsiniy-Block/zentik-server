namespace ZenticServer.PushEvents.Events;


// Типы событий
public enum EventTypes
{
    NewMessage,
    NewChat
}

public static class EventTypesExtension
{
    private static readonly Dictionary<EventTypes, string> DictToString = new ()
    {
        [EventTypes.NewMessage] = "new_message"
    };

    public static string ToString(EventTypes eventTypes)
    {
        return DictToString[eventTypes];
    }

}