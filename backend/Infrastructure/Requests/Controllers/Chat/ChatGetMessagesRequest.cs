using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Chat;

[SwaggerSchema(Required = new[] {
    "Get Chat Messages Request"
})]
[SwaggerTag("Chat")]
public class ChatGetMessagesRequest {

    public Infrastructure.Requests.Processor.Services.Chat.ChaGetMessagesRequest ToProcessorEntity(int userId, int sessionId, int? limit, int? lastMessageId) {
        return new Infrastructure.Requests.Processor.Services.Chat.ChaGetMessagesRequest {
            UserId = userId,
            SessionId = sessionId,
            Limit = limit,
            LastMessageId = lastMessageId
        };
    }
}