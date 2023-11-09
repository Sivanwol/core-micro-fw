using Domain.Entities.Session;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
using Domain.Responses.Controllers.Chat;
using Infrastructure.Enums;
using Infrastructure.Responses.Common;
namespace Domain.Persistence.Mock.Services;

public class MockChatServices : IChatMockService {
    public MockChatServices() {
        Faker = new SessionMockConfig();
        QuestionFaker = new QuestionsMockConfig();
        AppUserFaker = new AppUserMockConfig();
        QuestionAnswersFaker = new QuestionAnswersMockConfig();
        UserMessagesFaker = new UserMessagesMockConfig();
        UserAnswersFaker = new UserAnswersMockConfig();
        SessionFaker = new SessionMockConfig();
        MediaFaker = new MediaMockConfig();
    }
    private SessionMockConfig Faker { get; set; }
    private QuestionsMockConfig QuestionFaker { get; set; }
    private MediaMockConfig MediaFaker { get; set; }
    private QuestionAnswersMockConfig QuestionAnswersFaker { get; set; }
    private AppUserMockConfig AppUserFaker { get; set; }
    private SessionMockConfig SessionFaker { get; set; }
    private UserMessagesMockConfig UserMessagesFaker { get; set; }
    private UserAnswersMockConfig UserAnswersFaker { get; set; }
    public MessagesResponse GetMessages(int sessionId, int fromUserId, int toUserId) {
        var fromUser = AppUserFaker.Generate(1).Select(x => {
            x.Id = fromUserId.ToString();
            return x;
        }).First();
        var toUser = AppUserFaker.Generate(1).Select(x => {
            x.Id = toUserId.ToString();
            return x;
        }).First();
        Random random = new Random();
        var maxTotalMessages = random.Next(1, 4);
        var totalQuestions = 0;
        var messages = UserMessagesFaker.Generate(random.Next(10, 100)).Select(x => {
                x.SessionId = sessionId;
                x.UserId = x.ReplyId.HasValue ? toUserId : fromUserId;
                if (totalQuestions >= maxTotalMessages) {
                    x.QuestionId = null;
                    if (x.ReplyId.HasValue) {
                        x.Type = MessageType.Reply;
                    } else {
                        if (x.Type != MessageType.Message || x.Type != MessageType.BotMessage) {
                            x.Type = MessageType.Message;
                            x.ReplyId = null;
                        }
                    }
                    return x.ToChatMessage(null);
                }
                var question = QuestionFaker.Generate(1).First();
                var answer = QuestionAnswersFaker.Generate(1).Select(x => {
                    x.QuestionId = question.Id;
                    return x;
                }).First();
                var userAnswers = UserAnswersFaker.Generate(1).Select(x => {
                    x.QuestionId = question.Id;
                    x.AnswerId = answer.Id;
                    return x.ToChatQuestionAnswer();
                });
                var info = new QuestionInfo {
                    Question = question,
                    Answers = userAnswers
                };
                totalQuestions++;
                x.Type = MessageType.Question;

                return x.ToChatMessage(info);
            })
            .OrderBy(x => x.Type == MessageType.Question ? 0 : 1)
            .ThenBy(x => x.CreatedAt);
        var list = messages.ToList();
        return new MessagesResponse {
            MetaInfo = new MetaMessageInfo {
                TotalMessages = list.Count(),
                LastMessageDate = list.Last().CreatedAt,
                LastMessageId = list.Last().Id
            },
            Messages = list
        };
    }
}