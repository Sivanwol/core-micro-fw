using Domain.Entities;
using Domain.Entities.User;
namespace Domain.Persistence.Interfaces.Mock;

public interface ISessionsMockService {
    Sessions CreateSession(int ownerUserId, IEnumerable<int> matchUserIds);
    UserSessionInfo GetSessionInfo(int sessionId, int userId);
    UserSessionMessages GetSessionMessages(int sessionId, int userId);
    Dictionary<Questions, IEnumerable<QuestionAnswers>> GetNextQuestion(int sessionId, int userId);
}