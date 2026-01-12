namespace ZenticServer.PushEvents.Events;


// Типы событий
public enum Types
{
    NewMessage
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