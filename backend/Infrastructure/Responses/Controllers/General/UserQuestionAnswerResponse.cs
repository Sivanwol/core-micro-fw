namespace Infrastructure.Responses.Controllers.General;

public class UserQuestionAnswerResponse {
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
    public string Answer { get; set; }
    public int? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}