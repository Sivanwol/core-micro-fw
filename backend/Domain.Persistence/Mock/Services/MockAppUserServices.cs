using Domain.Entities;
using Domain.Entities.Matching;
using Domain.Entities.User;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Services;

public class MockAppUserServices : IAppUserMockService {
    public MockAppUserServices() {
        Faker = new AppUserMockConfig();
        SessionFaker = new SessionMockConfig();
        UserMatchingFaker = new UserMatchingMockConfig();
        MediaFaker = new MediaMockConfig();
        PartnerReligionsFaker = new PartnerReligionsMockConfig();
        PartnerEthnicitiesFaker = new PartnerEthnicitiesMockConfig();
        UserSessionFaker = new UserSessionMockConfig();
    }
    private AppUserMockConfig Faker { get; set; }
    private SessionMockConfig SessionFaker { get; set; }
    private UserMatchingMockConfig UserMatchingFaker { get; set; }
    private PartnerReligionsMockConfig PartnerReligionsFaker { get; set; }
    private PartnerEthnicitiesMockConfig PartnerEthnicitiesFaker { get; set; }
    private UserSessionMockConfig UserSessionFaker { get; set; }
    private MediaMockConfig MediaFaker { get; set; }

    public IEnumerable<UserPicks> GetPicks() {
        var picks = new List<UserPicks>();
        var newSession = SessionFaker.Generate(1).First();
        var mainUser = Faker.Generate(1).First();
        newSession.Status = SessionStatus.New;
        var newUsers = UserMatchingFaker.Generate(2).Select(x => {
            x.SessionId = newSession.Id;
            x.Status = UserMatchingStatus.New;
            x.UserId = mainUser.UserId;
            return x;
        });
        var openSession = SessionFaker.Generate(1).First();
        openSession.Status = SessionStatus.Open;
        var historyUsers = UserMatchingFaker.Generate(5).Select(x => {
            x.SessionId = openSession.Id;
            x.UserId = mainUser.UserId;
            if (x.Status != UserMatchingStatus.Reject) {
                x.Status = UserMatchingStatus.End;
            }
            return x;
        });
        var users = newUsers.Concat(historyUsers);
        users.ToList().ForEach(x => {
            var media = MediaFaker.Generate(1).First();
            var user = Faker.Generate(1).First();
            user.DefaultImageId = media.Id;
            x.MatchedUserId = user.UserId;
            var pick = new UserPicks {
                Match = x,
                MatchMedia = media,
                MatchUser = user
            };
            picks.Add(pick);
        });
        return picks;
    }
    public IEnumerable<UserPicks> GetSessionHistory() {
        var picks = new List<UserPicks>();
        var mainUser = Faker.Generate(1).First();
        var openSession = SessionFaker.Generate(1).First();
        openSession.Status = SessionStatus.Open;
        var historyUsers = UserMatchingFaker.Generate(5).Select(x => {
            x.SessionId = openSession.Id;
            if (x.Status != UserMatchingStatus.Reject) {
                x.Status = UserMatchingStatus.Reject;
            }
            return x;
        });
        historyUsers.ToList().ForEach(x => {
            var media = MediaFaker.Generate(1).First();
            var user = Faker.Generate(1).First();
            user.DefaultImageId = media.Id;
            x.MatchedUserId = user.UserId;
            var pick = new UserPicks {
                Match = x,
                MatchMedia = media,
                MatchUser = user
            };
            picks.Add(pick);
        });
        return picks;
    }

    public IEnumerable<UserMatches> CreateMatchingSession(SessionStatus status, int userId) {
        var openSession = SessionFaker.Generate(1).First();
        openSession.Status = SessionStatus.Open;

        var users = UserMatchingFaker.Generate(2).Select(x => {
            x.SessionId = openSession.Id;
            if (x.Status != UserMatchingStatus.Active) {
                x.Status = UserMatchingStatus.Active;
            }
            return x;
        });
        return users;
    }
    public UserProfile GetUserProfile(int id) {
        var user = Faker.Generate(1).First();
        user.UserId = id;
        var religions = PartnerReligionsFaker.Generate(2).Select(t => {
            t.UserId = user.UserId;
            return t;
        });
        var ethnicities = PartnerEthnicitiesFaker.Generate(2).Select(t => {
            t.UserId = user.UserId;
            return t;
        });
        return new UserProfile {
            User = user,
            PartnerReligions = religions,
            PartnerEthnicities = ethnicities
        };
    }
    public Users GetOne(int id) {
        var user = Faker.Generate(1).First();
        user.UserId = id;
        return user;
    }
    public Users GetOneByToken(string userToken) {
        var user = Faker.Generate(1).First();
        user.Token = userToken;
        return user;
    }
    public IEnumerable<UserSession> CreateUserSessions(IEnumerable<int> userIds) {
        var sessions = new List<UserSession>();
        var session = SessionFaker.Generate(1).First();
        session.Status = SessionStatus.Open;
        userIds.ToList().ForEach(x => {
            var userSession = new UserSession {
                UserId = x,
                SessionId = session.Id
            };
            sessions.Add(userSession);
        });
        return sessions;
    }
}