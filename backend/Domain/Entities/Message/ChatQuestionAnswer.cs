using Infrastructure.Responses.Controllers.General;
namespace Domain.Entities.Message;

public class ChatQuestionAnswer {
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
    public string Answer { get; set; }
    public int? Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public UserQuestionAnswerResponse ToResponse() {
        return new UserQuestionAnswerResponse {
            QuestionId = QuestionId,
            AnswerId = AnswerId,
            Answer = Answer,
            Rating = Rating,
            CreatedAt = CreatedAt
        };
    }
}