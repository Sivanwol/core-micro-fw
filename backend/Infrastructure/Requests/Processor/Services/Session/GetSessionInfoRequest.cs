using Infrastructure.Responses.Controllers.Session;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Session;

public class GetSessionInfoRequest : IRequest<SessionInfoResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
}