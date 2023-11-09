using Infrastructure.Enums;
using Infrastructure.Responses.Common;
using Infrastructure.Responses.Controllers.General;
namespace Infrastructure.Responses.Controllers.Session;

public class SessionInfoResponse {
    public SessionStatus Status { get; set; }
    public int FeedbackRating { get; set; }
    public IEnumerable<MatchingUser> Users { get; set; }
    public QuestionResponse CurrentQuestion { get; set; }
}