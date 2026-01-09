using System.Collections.Immutable;

namespace ZenticServer.Chat;

public class Types
{
    public static string PersonalMessage => "PersonalMessage";
    
    public static readonly IEnumerable<string> AllTypes = new HashSet<string>() {PersonalMessage};
}