using Infrastructure.Responses.Controllers.User;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class GetUserHistoryRequest : IRequest<GetUserSessionsResponse> {
    public int UserId { get; set; }
}