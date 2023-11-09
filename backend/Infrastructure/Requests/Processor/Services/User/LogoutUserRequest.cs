using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class LogoutUserRequest : IRequest<bool> {
    public string UserToken { get; set; }
}