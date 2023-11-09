using Domain.Entities.User;
using Domain.Persistence.Context;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Persistence.Repositories.Common;
using Infrastructure.Enums;
namespace Domain.Persistence.Repositories;

public class SessionRepository : BaseRepository, ISessionRepository {
    private readonly IAppUserMockService _appUserMock;
    private readonly ISessionsMockService _sessionsMock;

    public SessionRepository(IDomainContext context,
        ISessionsMockService sessionsMock,
        IAppUserMockService appUserMock) : base(context) {
        _sessionsMock = sessionsMock;
        _appUserMock = appUserMock;
    }

    public async Task<UserSessionInfo> AnswerQuestion(int sessionId, int userId, int questionId, int answerId) {
        return _sessionsMock.GetSessionInfo(sessionId, userId);
    }
    public async Task<UserSessionInfo> AnswerQuestion(int sessionId, int userId, int questionId, string answerText) {
        return _sessionsMock.GetSessionInfo(sessionId, userId);
    }
    public Task ReplyToQuestion(int sessionId, int userId, int questionId, string message) {
        return Task.CompletedTask;
    }
    public async Task<UserSessionInfo> RateQuestion(int sessionId, int userId, int questionId, int rating) {
        var result = _sessionsMock.GetSessionInfo(sessionId, userId);
        result.CurrentQuestion.Question.Id = questionId;
        return result;
    }
    public async Task<UserSessionInfo> MarkUnmatchSession(int sessionId, int userId, UnmatchReasonStatus status, string reason) {
        var result = _sessionsMock.GetSessionInfo(sessionId, userId);
        result.Status = Enum.GetName(SessionStatus.Reject)!;
        return result;
    }
    public async Task<UserSessionInfo> MarkMatchSession(int sessionId, int userId) {
        var result = _sessionsMock.GetSessionInfo(sessionId, userId);
        result.Status = Enum.GetName(SessionStatus.Match)!;
        return result;
    }
    public async Task<bool> HasSessionQuestionExist(int sessionId, int userId, int questionId) {
        return true;
    }
}