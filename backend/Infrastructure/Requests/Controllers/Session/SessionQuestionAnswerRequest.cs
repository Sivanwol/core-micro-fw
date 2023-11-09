using System.ComponentModel.DataAnnotations;
using Infrastructure.Requests.Processor.Services.Session;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Session;

[SwaggerSchema(Required = new[] {
    "Answer question request"
})]
[SwaggerTag("Session")]
public class SessionQuestionAnswerRequest {

    [Required]
    [SwaggerSchema("question id")]
    public int QuestionId { get; set; }

    [SwaggerSchema("answer id (can be null)")]
    public int? AnswerId { get; set; }

    [SwaggerSchema("answer text (can be null)")]
    public string? AnswerText { get; set; }

    public SessionAnswerQuestionRequest ToProcessorEntity(int userId, int sessionId) {
        return new SessionAnswerQuestionRequest {
            UserId = userId,
            SessionId = sessionId,
            QuestionId = QuestionId,
            AnswerId = AnswerId,
            AnswerText = AnswerText
        };
    }
}