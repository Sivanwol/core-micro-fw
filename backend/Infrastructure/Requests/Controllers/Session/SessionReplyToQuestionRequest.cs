using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Session;

[SwaggerSchema(Required = new[] {
    "Answer question request"
})]
[SwaggerTag("Session")]
public class SessionReplyToQuestionRequest {

    [Required]
    [SwaggerSchema("question id")]
    public int QuestionId { get; set; }

    [SwaggerSchema("text message")]
    public string Message { get; set; }

    public Infrastructure.Requests.Processor.Services.Session.SessionReplyToQuestionRequest ToProcessorEntity(int userId, int sessionId) {
        return new Infrastructure.Requests.Processor.Services.Session.SessionReplyToQuestionRequest {
            UserId = userId,
            SessionId = sessionId,
            QuestionId = QuestionId,
            Message = Message
        };
    }
}