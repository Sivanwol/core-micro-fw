using Domain.Entities;
using Domain.Entities.Matching;
using Domain.Entities.User;
using Infrastructure.Enums;
namespace Domain.Persistence.Interfaces.Mock;

public interface IAppUserMockService {
    IEnumerable<UserPicks> GetPicks();
    IEnumerable<UserPicks> GetSessionHistory();
    UserProfile GetUserProfile(int id);
    Users GetOne(int id);
    Users GetOneByToken(string userToken);
    IEnumerable<UserMatches> CreateMatchingSession(SessionStatus status, int userId);
    IEnumerable<UserSession> CreateUserSessions(IEnumerable<int> userIds);
}