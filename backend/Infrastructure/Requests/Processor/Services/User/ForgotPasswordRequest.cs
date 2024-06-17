using Infrastructure.Enums;
using Infrastructure.Responses.Controllers.Auth;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class ForgotPasswordRequest : IRequest<RequestOTPResponse> {
    public string BaseUrl { get; set; }
    public string? Email { get; set; }
    public int? CountryId { get; set; }
    public string? PhoneNumber { get; set; }
    public MFAProvider Provider { get; set; }
    public bool ReSend { get; set; }
}