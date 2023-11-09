using Domain.Entities;
using Domain.Entities.User;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
using Infrastructure.Enums;
using Infrastructure.Responses.Common;
namespace Domain.Persistence.Mock.Services;

public class MockSessionsServices : ISessionsMockService {
    public MockSessionsServices() {
        Faker = new SessionMockConfig();
        QuestionFaker = new QuestionsMockConfig();
        AppUserFaker = new AppUserMockConfig();
        QuestionAnswersFaker = new QuestionAnswersMockConfig();
        UserMessagesFaker = new UserMessagesMockConfig();
        UserAnswersFaker = new UserAnswersMockConfig();
        MediaFaker = new MediaMockConfig();
    }
    private SessionMockConfig Faker { get; set; }
    private QuestionsMockConfig QuestionFaker { get; set; }
    private MediaMockConfig MediaFaker { get; set; }
    private QuestionAnswersMockConfig QuestionAnswersFaker { get; set; }
    private AppUserMockConfig AppUserFaker { get; set; }
    private UserMessagesMockConfig UserMessagesFaker { get; set; }
    private UserAnswersMockConfig UserAnswersFaker { get; set; }

    public Sessions CreateSession(int ownerUserId, IEnumerable<int> matchUserIds) {
        return Faker.Generate(1).First();
    }

    public UserSessionInfo GetSessionInfo(int sessionId, int userId) {
        var session = Faker.Generate(1).First();
        session.Id = sessionId;
        var currentQuestion = QuestionFaker.Generate(1).Select(q => {
            var random = new Random();
            var answers = QuestionAnswersFaker.Generate(random.Next(2, 6)).Select(x => {
                x.QuestionId = q.Id;
                return x;
            });
            return new QuestionInfo {
                Question = q,
                Answers = answers.OrderBy(x => x.CreatedAt).ToList(),
            };
        }).First();
        var ownerUser = AppUserFaker.Generate(1).Select(x => {
            x.UserId = userId;
            var media = MediaFaker.Generate(1).First();
            return x.ToUserProfileShortInfo(media);
        }).First();
        var matchingUser = AppUserFaker.Generate(1).Select(x => {
            var media = MediaFaker.Generate(1).First();
            return x.ToUserProfileShortInfo(media);
        }).First();
        var userSession = new UserSessionInfo {
            Session = session,
            Status = Enum.GetName(SessionStatus.New)!,
            CurrentQuestion = currentQuestion,
            MatchUserIds = new List<MatchingUser> {
                ownerUser.ToResponse(),
                matchingUser.ToResponse()
            }
        };
        return userSession;
    }

    public UserSessionMessages GetSessionMessages(int sessionId, int userId) {
        var session = Faker.Generate(1).First();
        session.Id = sessionId;
        var userProflies = AppUserFaker.Generate(2).Select(x => {
            x.UserId = userId;
            var media = MediaFaker.Generate(1).First();
            return x.ToUserProfileShortInfo(media);
        });
        var messages = UserMessagesFaker.Generate(10).Select(x => {
            x.SessionId = sessionId;
            return x;
        });
        var userMessages = new UserSessionMessages {
            Session = session,
            Messages = messages,
            MessagingUserProfiles = userProflies
        };
        return userMessages;
    }
    public Dictionary<Questions, IEnumerable<QuestionAnswers>> GetNextQuestion(int sessionId, int userId) {
        var currentQuestion = QuestionFaker.Generate(1).Select(q => {
            var answers = QuestionAnswersFaker.Generate(4);
            var question = new Dictionary<Questions, IEnumerable<QuestionAnswers>>();
            question.Add(q, answers);
            return question;
        }).First();
        return currentQuestion;
    }
}