using System.Text;

namespace ZenticServer.PushEvents;


/*
  Класс для управления пулами сессий SSE
  Singleton
*/
public class SseSessionManager
{
    private readonly List<SseSessionPull> _channels;
    private readonly SseSettings _settings;
    private readonly Task _keepAlive;
    
    public SseSessionManager(SseSettings settings)
    {
        _settings = settings;
        _keepAlive = KeepAlive();
        _channels = new (5);
        for (int i=0; i<_settings.ChannelsCount; i++)
        {
            _channels.Add(new SseSessionPull());
        }
    }
    
    public void AddSession(HttpResponse httpResponse, int userId, int deviceId)
    {
        var currentChannel = _channels[userId % _settings.ChannelsCount];
        currentChannel.SendEvent(new AddSessionEvent(userId, deviceId, httpResponse));
    }

    public void Send(string message, Events.Types types, int userId)
    {
        _channels[userId % _settings.ChannelsCount].SendEvent(
            new SendSessionEvent(
                $"""
                 event: {Events.TypesExtension.ToString(types)}
                 data:{message}
                 """, 
                userId));
    }

    private async Task KeepAlive()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.KeepAliveTimer));
        
            var temp = new KeepAliveSessionEvent();
            foreach (var channel in _channels)
            {
                channel.SendEvent(temp);
            }
        }
    }
}