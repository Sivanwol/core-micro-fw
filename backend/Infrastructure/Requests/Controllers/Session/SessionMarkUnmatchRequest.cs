using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Session;

[SwaggerSchema(Required = new[] {
    "Mark session as un match request"
})]
[SwaggerTag("Session")]
public class SessionMarkUnmatchRequest {

    [Required]
    [SwaggerSchema("unmatch status")]
    public string Status { get; set; }

    [Required]
    [SwaggerSchema("reason for unmatch")]
    public string Reason { get; set; }

    public Infrastructure.Requests.Processor.Services.Session.SessionMarkAsUnMatchRequest ToProcessorEntity(int userId, int sessionId) {
        return new Infrastructure.Requests.Processor.Services.Session.SessionMarkAsUnMatchRequest {
            UserId = userId,
            SessionId = sessionId,
            Status = Enum.Parse<UnmatchReasonStatus>(Status),
            Reason = Reason
        };
    }
}