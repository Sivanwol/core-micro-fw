using Infrastructure.Responses.Controllers.Session;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Session;

public class SessionQuestionRateRequest : IRequest<SessionInfoResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int QuestionId { get; set; }
    public int FeedbackRating { get; set; }
}