using System.Collections.Concurrent;

namespace ZenticServer.Message;

public class MessageRepository : IMessageRepository
{
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, MessageModel>> _messages = [];
    public async Task<MessageModel> CreateMessage(string text, int senderId, int chatId)
    {
        var newMessage = new MessageModel(
            text, 
            DateTime.Now,
            Guid.NewGuid().GetHashCode(), 
            senderId, 
            chatId);
        _messages[chatId][newMessage.MessageId] = newMessage;
        return newMessage;
    }

    public Task<List<MessageModel>> GetMessages(int chatId, int offset, int limit)
    {
        if (!_messages.TryGetValue(chatId, out var chat))
            throw new KeyNotFoundException();

        var messages = chat.Values.ToList();
        // Сортировка по времени т.к. в качестве id используется UUID
        messages.Sort((model, messageModel) => model.SendTime.CompareTo(messageModel));
        return Task.FromResult(messages);
    }
}