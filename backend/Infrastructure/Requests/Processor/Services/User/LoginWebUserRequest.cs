using Infrastructure.Enums;
using Infrastructure.Responses.Controllers.Auth;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class LoginWebUserRequest : IRequest<RequestOTPResponse> {
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public MFAProvider Provider { get; set; }
    public bool ReSend { get; set; }
}