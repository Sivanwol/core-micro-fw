using Domain.Entities;
using Domain.Entities.Matching;
using Domain.Entities.User;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IAppUserRepository : IGenericRepository<Users> {
    Task<IEnumerable<UserPicks>> GetRecommendPicks(int userId);
    Task<IEnumerable<UserPicks>> GetSessionHistory(int userId);
    Task RegisterUserInit(NewUserInitRequest request);
    Task NewUserProfileInfo(NewUserRegisterInfoRequest request);
    Task NewUserProfilePreference(NewUserRegisterPreferenceRequest request);
    Task UpdateUserPhotos(UpdateUserRegisterMediaRequest request);
    Task UpdateUserProfile(UserUpdateProfileRequest request);
    Task<UserSessionInfo> StartSession(int ownerUserId, IEnumerable<int> matchUserIds);
    Task<UserSessionInfo> GetSessionInfo(int sessionId, int userId);
    Task MarkUnmatchSession(int sessionId, int userId, UnmatchReasonStatus status, string reason); // this will close session
    Task MarkMatchSession(int sessionId, int userId);
    Task DeleteUser(int userId);
    Task ExportUser(int userId);
    Task<Users> GetUserByToken(string userToken);
    Task<UserProfile> GetUserProfile(int id);
    Task<bool> HasUserSessionExist(int sessionId, int userId);
}