using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class LogoutUserRequest : BaseRequest<bool> {
    public string UserToken { get; set; }
}