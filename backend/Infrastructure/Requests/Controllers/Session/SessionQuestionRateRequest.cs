using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Session;

[SwaggerSchema(Required = new[] {
    "Answer question request"
})]
[SwaggerTag("Session")]
public class SessionQuestionRateRequest {

    [Required]
    [SwaggerSchema("question id")]
    public int QuestionId { get; set; }

    [SwaggerSchema("rate question (1-5)")]
    public int Rate { get; set; }

    public Infrastructure.Requests.Processor.Services.Session.SessionQuestionRateRequest ToProcessorEntity(int userId, int sessionId) {
        return new Infrastructure.Requests.Processor.Services.Session.SessionQuestionRateRequest {
            UserId = userId,
            SessionId = sessionId,
            QuestionId = QuestionId,
            FeedbackRating = Rate
        };
    }
}