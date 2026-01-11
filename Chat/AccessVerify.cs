namespace ZenticServer.Chat;

public static class AccessVerify
{
    // Проверка доступа пользователю на действия в чате
    // Не проверяет находиться ли человек в чате 
    
    public static bool ClearChat(string chatType, string userRole)
    {
        return chatType == Types.PersonalMessage ||
            (chatType == Types.Group && userRole == ChatUser.Role.GroupAdmin);
    }

    public static bool DeleteChat(string chatType, string userRole)
    {
        return chatType == Types.PersonalMessage ||
               (chatType == Types.Group && userRole == ChatUser.Role.GroupAdmin);
    }

    public static bool DeleteMessage(string chatType, string userRole)
    {
        return true;
    }
    public static bool EditMessage(string chatType, string userRole)
    {
        return true;
    }
    
    
}