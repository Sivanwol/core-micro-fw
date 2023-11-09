using Domain.Persistence.Context;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Persistence.Repositories.Common;
using Domain.Responses.Controllers.Chat;
using Infrastructure.Enums;
using Microsoft.Extensions.Logging;
namespace Domain.Persistence.Repositories;

public class ChatRepository : BaseRepository, IChatRepository {
    private readonly ILogger _logger;
    private readonly IChatMockService _mock;
    public ChatRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory,
        IChatMockService mock
    ) : base(context) {
        _logger = loggerFactory.CreateLogger<ChatRepository>();
        _mock = mock;
    }

    public Task DeleteMessage(int sessionId, int userId, int messageId) {
        _logger.LogInformation($"DeleteMessage: {sessionId}, {userId}, {messageId}");
        return Task.CompletedTask;
    }
    public Task EditMessage(int sessionId, int userId, int messageId, string message) {
        _logger.LogInformation($"EditMessage: {sessionId}, {userId}, {messageId}");
        return Task.CompletedTask;
    }
    public Task SendMessage(int sessionId, int fromUserId, string message) {
        _logger.LogInformation($"SendMessage: {sessionId}, {fromUserId}");
        return Task.CompletedTask;
    }
    public Task SendMessage(int sessionId, int userId, string message, MessageType type) {
        _logger.LogInformation($"SendMessage: {sessionId}, {userId}, {type}");
        return Task.CompletedTask;
    }
    public Task ReplyMessage(int sessionId, int userId, int messageId, string message) {
        _logger.LogInformation($"ReplyMessage: {sessionId}, {userId}, {messageId}");
        return Task.CompletedTask;
    }
    public Task ReportUser(int sessionId, int userId, int reportUserId, ReportFlag reportFlag, string? reportMessage) {
        _logger.LogInformation($"ReportUser: {sessionId}, {userId}, {reportUserId}, {reportFlag}");
        return Task.CompletedTask;
    }
    public Task<MessagesResponse> GetMessages(int sessionId, int userId) {
        _logger.LogInformation($"GetMessages: [{sessionId}, {userId}]");
        return Task.FromResult(_mock.GetMessages(sessionId, userId, 2));
    }
    public Task<MessagesResponse> GetMessages(int sessionId, int userId, int? limit, int? lastMessageId) {
        _logger.LogInformation($"GetMessages: [{sessionId}, {userId}, {limit}, {lastMessageId}]");
        return Task.FromResult(_mock.GetMessages(sessionId, userId, 2));
    }
    public Task<bool> HasMessage(int sessionId, int userId, int messageId) {
        return Task.FromResult(true);
    }
}