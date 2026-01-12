namespace ZenticServer.Chat.Pm;

public interface IRepository
{
    public Task<CreatePmDto> Create(int firstUserId, int secondUserId);
    public class CreatePmDto
    {
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string FirstMessageText { get; set; }
        public string FirstMessageSender { get; set; }
    }

    
}