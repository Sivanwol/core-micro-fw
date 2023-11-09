using Infrastructure.Responses.Common;
namespace Domain.Entities.User;

public class UserSessionInfo {
    public Sessions Session { get; set; }
    public string Status { get; set; }
    public QuestionInfo CurrentQuestion { get; set; }
    public IEnumerable<MatchingUser> MatchUserIds { get; set; }
}