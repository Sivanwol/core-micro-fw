using Application.Exceptions;
using Domain.Entities;
using Domain.Entities.Matching;
using Domain.Entities.User;
using Domain.Persistence.Context;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Persistence.Repositories.Common;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Serilog;
namespace Domain.Persistence.Repositories;

public class AppUserRepository : BaseRepository, IAppUserRepository {
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILanguagesRepository _languagesRepository;
    private readonly IAppUserMockService _mock;
    private readonly ISessionsMockService _sessionsMock;
    public AppUserRepository(
        IDomainContext context,
        ICountriesRepository countriesRepository,
        ILanguagesRepository languagesRepository,
        ISessionsMockService sessionsMock,
        IAppUserMockService mock
    ) : base(context) {
        _mock = mock;
        _sessionsMock = sessionsMock;
        _countriesRepository = countriesRepository;
        _languagesRepository = languagesRepository;
    }

    public async Task RegisterUserInit(NewUserInitRequest request) {
        Log.Logger.Information($"Registering user with phone number {request.PhoneNumber}");
        var countryData = await _countriesRepository.GetById(request.CountryId);
        var languageData = await _languagesRepository.GetById(request.LanguageId);

        if (countryData == null) {
            throw new EntityNotFoundException("Countries", request.CountryId.ToString());
        }
        if (languageData == null) {
            throw new EntityNotFoundException("Languages", request.LanguageId.ToString());
        }
    }
    public async Task NewUserProfileInfo(NewUserRegisterInfoRequest request) {
        Log.Logger.Information($"Updating new user with id {request.UserId}");
        var user = await GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
    }
    public async Task NewUserProfilePreference(NewUserRegisterPreferenceRequest request) {
        Log.Logger.Information($"Updating new user with id {request.UserId}");
        var user = await GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
    }
    public async Task UpdateUserPhotos(UpdateUserRegisterMediaRequest request) {
        Log.Logger.Information($"Updating new user with id {request.UserId}");
        var user = await GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
    }
    public async Task UpdateUserProfile(UserUpdateProfileRequest request) {
        Log.Logger.Information($"Updating new user with id {request.UserId}");
        var user = await GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
    }
    public async Task DeleteUser(int userId) {
        Log.Logger.Information($"Delete new user with id {userId}");
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }

    }
    public async Task ExportUser(int userId) {
        Log.Logger.Information($"Export new user with id {userId}");
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
    }

    public Task<UserProfile> GetUserProfile(int id) {
        Log.Logger.Information($"Fetching user with id {id}");
        return Task.FromResult(_mock.GetUserProfile(id));
    }

    public Task<Users> GetById(int id) {
        Log.Logger.Information($"Fetching user with id {id}");
        return Task.FromResult(_mock.GetOne(id));
    }

    public Task<IEnumerable<UserPicks>> GetRecommendPicks(int userId) {
        Log.Logger.Information("Fetching all recommends picks {userId}", userId);
        return Task.FromResult(_mock.GetPicks());
    }

    public Task<IEnumerable<UserPicks>> GetSessionHistory(int userId) {
        Log.Logger.Information("Fetching all session history {userId}", userId);
        return Task.FromResult(_mock.GetSessionHistory());
    }

    public Task<Users> GetUserByToken(string userToken) {
        Log.Logger.Information($"Fetching user with token {userToken}");
        return Task.FromResult(_mock.GetOneByToken(userToken));
    }
    public Task<UserSessionInfo> StartSession(int ownerUserId, IEnumerable<int> matchUserIds) {
        Log.Logger.Information($"Starting session for user {ownerUserId}");
        var session = _sessionsMock.CreateSession(ownerUserId, matchUserIds);
        var sessionInfo = _sessionsMock.GetSessionInfo(session.Id, ownerUserId);
        return Task.FromResult(sessionInfo);
    }
    public Task<UserSessionInfo> GetSessionInfo(int sessionId, int userId) {
        Log.Logger.Information($"Fetching session {sessionId} for user {userId}");
        var sessionInfo = _sessionsMock.GetSessionInfo(sessionId, userId);
        return Task.FromResult(sessionInfo);
    }
    public async Task MarkUnmatchSession(int sessionId, int userId, UnmatchReasonStatus status, string reason) {
        Log.Logger.Information($"Marking session {sessionId} as unmatched for user {userId}");
        if (!(await HasUserSessionExist(sessionId, userId))) {
            throw new EntityNotFoundException("UserSession", $"{sessionId}-{userId}");
        }
        Log.Logger.Information($"Session {sessionId} is unmatched for user {userId}");
    }
    public async Task MarkMatchSession(int sessionId, int userId) {
        Log.Logger.Information($"Marking session {sessionId} as matched for user {userId}");
        if (!(await HasUserSessionExist(sessionId, userId))) {
            throw new EntityNotFoundException("UserSession", $"{sessionId}-{userId}");
        }
        Log.Logger.Information($"Session {sessionId} is matched for user {userId}");
    }
    public Task<bool> HasUserSessionExist(int sessionId, int userId) {
        Log.Logger.Information($"Checking if session {sessionId} exist for user {userId}");
        return Task.FromResult(true);
    }
}