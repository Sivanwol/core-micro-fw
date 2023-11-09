using Domain.Entities;
using Domain.Entities.User;
using Infrastructure.Enums;
namespace Domain.Persistence.Interfaces.Repositories;

public interface ISessionRepository : IGenericEmptyRepository<Sessions> {
    Task<UserSessionInfo> AnswerQuestion(int sessionId, int userId, int questionId, int answerId);
    Task<UserSessionInfo> AnswerQuestion(int sessionId, int userId, int questionId, string answerText);
    Task ReplyToQuestion(int sessionId, int userId, int questionId, string message);
    Task<UserSessionInfo> RateQuestion(int sessionId, int userId, int questionId, int rating);
    Task<UserSessionInfo> MarkUnmatchSession(int sessionId, int userId, UnmatchReasonStatus status, string reason);
    Task<UserSessionInfo> MarkMatchSession(int sessionId, int userId);
    Task<bool> HasSessionQuestionExist(int sessionId, int userId, int questionId);
}