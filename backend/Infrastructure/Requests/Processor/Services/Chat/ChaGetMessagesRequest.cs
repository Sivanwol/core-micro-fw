namespace Infrastructure.Requests.Processor.Services.Chat;

public class ChaGetMessagesRequest {
    public int SessionId { get; set; }
    public int UserId { get; set; }
    public int? Limit { get; set; }
    public int? LastMessageId { get; set; }
}