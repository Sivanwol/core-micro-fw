using Infrastructure.Responses.Controllers.Auth;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class VerifyOTPRequest : IRequest<VerifyOTPResponse> {
    public string Code { get; set; }
    public string OTPToken { get; set; }
    public string UserToken { get; set; }
}