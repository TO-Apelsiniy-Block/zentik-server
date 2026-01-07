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
    
    public async Task AddSession(HttpResponse httpResponse, int userId, int deviceId)
    {
        var currentChannel = _channels[userId % _settings.ChannelsCount];
        
        currentChannel.SendEvent(new AddSessionEvent(userId, deviceId, httpResponse));
    }

    public async Task Send(string message, EventTypes eventTypes, int userId)
    {
        // TODO мб надо отправлять еще и тип?
        _channels[userId % _settings.ChannelsCount].SendEvent(
            new SendSessionEvent(
                $"event: {EventTypesExtension.ToString(eventTypes)}\ndata:{message}\n\n", 
                userId));
    }
}