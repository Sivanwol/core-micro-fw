using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Chat;

[SwaggerSchema(Required = new[] {
    "Get Chat Messages Request"
})]
[SwaggerTag("Chat")]
public class ChatEditMessageRequest {
    [Required]
    [SwaggerSchema("Message text")]
    public string Message { get; set; }

    [Required]
    [SwaggerSchema("message id")]
    public int MessageId { get; set; }

    public Infrastructure.Requests.Processor.Services.Chat.ChatEditMessageRequest ToProcessorEntity(int userId, int sessionId) {
        return new Infrastructure.Requests.Processor.Services.Chat.ChatEditMessageRequest {
            UserId = userId,
            SessionId = sessionId,
            Message = Message,
            MessageId = MessageId
        };
    }
}