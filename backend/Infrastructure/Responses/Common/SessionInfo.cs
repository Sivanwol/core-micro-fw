using Infrastructure.Enums;
namespace Infrastructure.Responses.Common;

public class SessionInfo {
    public int SessionId { get; set; }
    public int FeedbackRating { get; set; }
    public SessionStatus Status { get; set; }
}