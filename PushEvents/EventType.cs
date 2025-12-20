using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ZenticServer.PushEvents;


// Типы событий
public enum EventType
{
    NewMessage,
    NewChat
}

public static class EventTypeExtension
{
    private static readonly Dictionary<EventType, string> DictToString = new ()
    {
        [EventType.NewMessage] = "new_message"
    };

    public static string ToString(EventType eventType)
    {
        return DictToString[eventType];
    }

}