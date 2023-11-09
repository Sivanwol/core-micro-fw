using Domain.Entities.User;
using Domain.Responses.Controllers.Chat;
using Infrastructure.Enums;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IChatRepository : IGenericEmptyRepository<UserSessionMessages> {
    Task DeleteMessage(int sessionId, int userId, int messageId);
    Task EditMessage(int sessionId, int userId, int messageId, string message);
    Task SendMessage(int sessionId, int userId, string message);
    Task SendMessage(int sessionId, int userId, string message, MessageType type);
    Task ReplyMessage(int sessionId, int userId, int messageId, string message);
    Task ReportUser(int sessionId, int userId, int reportUserId, ReportFlag reportFlag, string? reportMessage);
    Task<MessagesResponse> GetMessages(int sessionId, int userId);
    Task<MessagesResponse> GetMessages(int sessionId, int userId, int? limit, int? lastMessageId);
    Task<bool> HasMessage(int sessionId, int userId, int messageId);
}