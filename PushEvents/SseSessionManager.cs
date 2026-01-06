using System.Collections.Concurrent;
using System.Security.AccessControl;

namespace ZenticServer.PushEvents;


/*
  Класс для управления пулами сессий SSE
  Singleton
*/
public class SseSessionManager
{
    private readonly List<SseSessionPull> _channels;
    private readonly SseSettings _settings;
    
    public SseSessionManager(SseSettings settings)
    {
        _settings = settings;
        
        _channels = new (5);
        for (int i=0; i<_settings.ChannelsCount; i++)
        {
            _channels.Add(new SseSessionPull());
        }
        
    }
    
    public void AddSession(HttpContext httpContext, int chatId, int deviceId)
    {
        var currentChannel = _channels[chatId % _settings.ChannelsCount];
    }

    public async Task SendMessage(string message, Types types, int chatId)
    {
        await _channels[0].SendEvent(new SendSessionEvent());
    }
}