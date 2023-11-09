using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities.Message;
namespace Domain.Entities;

[Table("UserAnswer")]
public class UserAnswer : BaseEntity {
    public int UserMessageId { get; set; }
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }

    [Column("Text")]
    public string Answer { get; set; }

    public int? Rating { get; set; }

    public ChatQuestionAnswer ToChatQuestionAnswer() {
        return new ChatQuestionAnswer {
            QuestionId = QuestionId,
            AnswerId = AnswerId,
            Answer = Answer,
            Rating = Rating
        };
    }
}