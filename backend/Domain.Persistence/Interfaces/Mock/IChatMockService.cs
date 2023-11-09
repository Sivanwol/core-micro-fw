using Domain.Responses.Controllers.Chat;
namespace Domain.Persistence.Interfaces.Mock;

public interface IChatMockService {
    MessagesResponse GetMessages(int sessionId, int fromUserId, int toUserId);
}