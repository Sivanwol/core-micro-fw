using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class UpdateUserProfileRequest : BaseRequest<GQL.User> {
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public string Address { get; set; }
}