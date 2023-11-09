using Domain.Entities.Session;
using Infrastructure.Enums;
namespace Domain.Entities.Message;

public class ChatMessage {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public QuestionInfo? QuestionInfo { get; set; }
    public int? ReplyId { get; set; }
    public string Message { get; set; }
    public MessageType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}