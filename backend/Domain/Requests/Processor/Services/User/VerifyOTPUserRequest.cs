using Infrastructure.Responses.Controllers.Auth;
using MediatR;
namespace Domain.Requests.Processor.Services.User;

public class VerifyOtpUserRequest : IRequest<VerifyOTPResponse> {
    public string Code { get; set; }
    public string OTPToken { get; set; }
    public string UserToken { get; set; }
}