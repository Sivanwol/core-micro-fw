using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Chat;

[SwaggerSchema(Required = new[] {
    "Get Chat Messages Request"
})]
[SwaggerTag("Chat")]
public class ChatFlagUserRequest {
    [Required]
    [SwaggerSchema("report reason text")]
    public string Reason { get; set; }

    [Required]
    [SwaggerSchema("report on user id")]
    public int ReportUserId { get; set; }

    [Required]
    [SwaggerSchema("what type of report is this?")]
    public string ReportType { get; set; }

    public Infrastructure.Requests.Processor.Services.Chat.ChatReportUserRequest ToProcessorEntity(int userId, int sessionId) {
        return new Infrastructure.Requests.Processor.Services.Chat.ChatReportUserRequest {
            UserId = userId,
            SessionId = sessionId,
            Reason = Reason,
            ReportedUserId = ReportUserId,
            ReportType = Enum.Parse<ReportFlag>(ReportType)
        };
    }
}