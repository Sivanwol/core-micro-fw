using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class GetUserProfileRequest : BaseRequest<Infrastructure.GQL.User> {
    public Guid UserId { get; set; }
}