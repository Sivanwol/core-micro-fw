using System.ComponentModel;
using Infrastructure.Requests.Processor.Services.User;
namespace Infrastructure.GQL.Inputs.User;

[Description("Update my user profile")]
public class UpdateUserProfileInput {
    [Description("User first name")]
    public string FirstName { get; set; }
    [Description("User last name")]
    public string LastName { get; set; }
    [Description("User phone number (may not verify so wont be entered)")]
    public string PhoneNumber { get; set; }
    [Description("User country")]
    public int CountryId { get; set; }
    public string Address { get; set; }

    public UpdateUserProfileRequest ToProcessorEntity(Guid loggedInUserId, string ipAddress, string userAgent) {
        return new UpdateUserProfileRequest {
            UserId = loggedInUserId,
            Address = Address,
            CountryId = CountryId,
            PhoneNumber = PhoneNumber,
            FirstName = FirstName,
            LastName = LastName,
            LoggedInUserId = loggedInUserId,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }
}