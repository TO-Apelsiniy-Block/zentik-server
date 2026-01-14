namespace ZenticServer;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

public class SseSettings
{
    public int ChannelsCount { get; set; } = 1;
    public int KeepAliveTimer { get; set; } = 10;
}

public class DbSettings
{
    // TODO убрать пароль куда-то
    public string ConnectionString { get; set; } = string.Empty;
}

public class EmailSettings
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Security { get; set; } = string.Empty;
}