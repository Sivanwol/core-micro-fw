using Infrastructure.Responses.App.Users;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User; 

public class GetUserProfileRequest : IRequest<ProfileResponse>{
    public int UserId { get; set; }
}