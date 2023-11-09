using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Chat;

[SwaggerSchema(Required = new[] {
    "Get Chat Messages Request"
})]
[SwaggerTag("Chat")]
public class ChatSendMessageRequest {
    [Required]
    [SwaggerSchema("Message text")]
    public string Message { get; set; }

    [SwaggerSchema("if this message is a reply to another message then this field should be filled with the id of the message that is being replied to")]
    public int? ReplyMessageId { get; set; }

    public Infrastructure.Requests.Processor.Services.Chat.ChatSendMessageRequest ToProcessorEntity(int userId, int sessionId, MessageType messageType) {
        return new Infrastructure.Requests.Processor.Services.Chat.ChatSendMessageRequest {
            UserId = userId,
            SessionId = sessionId,
            Message = Message,
            Type = messageType,
            ReplyId = ReplyMessageId
        };
    }
}