using System.ComponentModel.DataAnnotations;
using Infrastructure.Requests.Processor.Services.User;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.User;

[SwaggerSchema(Required = new[] {
    "Update user profile request"
})]
[SwaggerTag("Controllers")]
public class UpdateUserProfileRequest {
    [Required]
    [SwaggerSchema("Email")]
    public string Email { get; set; }

    [Required]
    [SwaggerSchema("First Name")]
    public string FirstName { get; set; }

    [Required]
    [SwaggerSchema("Last Name")]
    public string LastName { get; set; }

    [Required]
    [SwaggerSchema("Latitude")]
    public double Latitude { get; set; }

    [Required]
    [SwaggerSchema("Longitude")]
    public double Longitude { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [SwaggerSchema("Phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    [SwaggerSchema("Country Id")]
    public int CountryId { get; set; }

    [Required]
    [SwaggerSchema("Language Id")]
    public int LanguageId { get; set; }

    [Required]
    [SwaggerSchema("Birth Date")]
    public DateTime BirthDate { get; set; }

    [Required]
    [SwaggerSchema("User Personal Info")]
    public UpdateRegisterNewUserInfoRequest PresonalInfo { get; set; }

    [Required]
    [SwaggerSchema("User Preference Info")]
    public UpdateRegisterNewUserPreferenceRequest Preference { get; set; }

    public UserUpdateProfileRequest ToProcessorEntity(int userId) {
        var preference = Preference.ToProcessorUpdateEntity();
        var personalInfo = PresonalInfo.ToProcessorUpdateEntity();
        return new UserUpdateProfileRequest {
            Email = Email,
            PhoneNumber = PhoneNumber,
            CountryId = CountryId,
            LanguageId = LanguageId,
            Latitude = Latitude,
            Longitude = Longitude,
            LastName = LastName,
            FirstName = FirstName,
            BirthDate = BirthDate,
            Preference = preference,
            PresonalInfo = personalInfo,
            UserId = userId
        };
    }
}