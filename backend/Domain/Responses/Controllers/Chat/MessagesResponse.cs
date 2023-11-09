using Domain.Entities.Message;
namespace Domain.Responses.Controllers.Chat;

public class MessagesResponse : Infrastructure.Responses.Controllers.Chat.MessagesResponse {
    public IEnumerable<ChatMessage> Messages { get; set; }
}