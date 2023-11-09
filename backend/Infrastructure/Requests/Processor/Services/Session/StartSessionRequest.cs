using Infrastructure.Responses.Controllers.Session;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Session;

public class StartSessionRequest : IRequest<SessionInfoResponse> {
    public int UserId { get; set; }
    public IEnumerable<int> MatchingUserIds { get; set; }
}