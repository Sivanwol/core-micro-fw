using Infrastructure.Responses.Controllers.User;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class GetUserPicksRequest : IRequest<GetUserPickResponse> {
    public int UserId { get; set; }
}