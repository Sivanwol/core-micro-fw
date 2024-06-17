using Application.Responses.Base;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class RegisterRequest : IRequest<EmptyResponse> {
    public Guid OwnerUserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public int? CountryId { get; set; }
    public int DisplayLanguageId { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string BaseUrl { get; set; }
}