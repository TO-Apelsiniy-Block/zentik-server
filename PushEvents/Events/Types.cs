using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ZenticServer.PushEvents;


// Типы событий
public enum Types
{
    NewMessage,
    NewChat
}

public static class TypesExtension
{
    private static readonly Dictionary<Types, string> DictToString = new ()
    {
        [Types.NewMessage] = "new_message"
    };

    public static string ToString(Types types)
    {
        return DictToString[types];
    }

}