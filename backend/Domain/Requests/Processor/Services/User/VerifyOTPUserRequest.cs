using Infrastructure.Responses.Controllers.Auth;
using MediatR;
namespace Domain.Requests.Processor.Services.User;

public enum ActionOtpType {
    Login,
    ForgotPassword,
}

public class VerifyOtpUserRequest : IRequest<VerifyOTPResponse> {
    public string Code { get; set; }
    public string OTPToken { get; set; }
    public string UserToken { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public ActionOtpType ActionOtpType { get; set; }
}