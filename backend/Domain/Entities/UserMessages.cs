using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities.Message;
using Domain.Entities.Session;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("UserMessages")]
public class UserMessages : BaseEntity {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int? QuestionId { get; set; }

    [Column("ReplyToMessageId")]
    public int? ReplyId { get; set; }

    public string Message { get; set; }
    public MessageType Type { get; set; }
    public ChatMessage ToChatMessage(QuestionInfo? question) {
        return new ChatMessage {
            Id = Id,
            UserId = UserId,
            SessionId = SessionId,
            QuestionInfo = question,
            ReplyId = ReplyId,
            Message = Message,
            Type = Type,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}