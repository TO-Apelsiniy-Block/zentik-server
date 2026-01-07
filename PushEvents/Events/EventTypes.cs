using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ZenticServer.PushEvents;


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