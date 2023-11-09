using Infrastructure.Enums;
using Infrastructure.Responses.Controllers.Session;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Session;

public class SessionMarkAsUnMatchRequest : IRequest<SessionInfoResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public UnmatchReasonStatus Status { get; set; }
    public string Reason { get; set; }
}