using Infrastructure.Enums;
using Infrastructure.Responses.Controllers.Auth;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class SendOTPRequest : IRequest<RequestOTPResponse> {
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public MFAProvider Provider { get; set; }
}