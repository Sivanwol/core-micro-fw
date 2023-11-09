using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Chat;

[SwaggerSchema(Required = new[] {
    "Get Chat Messages Request"
})]
[SwaggerTag("Chat")]
public class ChatDeleteMessageRequest {

    [Required]
    [SwaggerSchema("message id")]
    public int MessageId { get; set; }

    public Infrastructure.Requests.Processor.Services.Chat.ChatDeleteMessageRequest ToProcessorEntity(int userId, int sessionId) {
        return new Infrastructure.Requests.Processor.Services.Chat.ChatDeleteMessageRequest {
            UserId = userId,
            SessionId = sessionId,
            MessageId = MessageId
        };
    }
}