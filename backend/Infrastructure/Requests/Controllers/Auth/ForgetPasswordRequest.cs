using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Forget User Password"
})]
[SwaggerTag("Auth Controllers")]
public class ForgetPasswordRequest {

    [SwaggerSchema("Email of the user")]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [SwaggerSchema("Country Id of the user")]
    [Required]
    public int? CountryId { get; set; }

    [SwaggerSchema("Phone of the user")]
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    [SwaggerSchema("Provider of the MFA")]
    public string Provider { get; set; }

    [Required]
    [SwaggerSchema("Resent OTP")]
    public bool Resent { get; set; }

    public ForgotPasswordRequest ToProcessorEntity() {
        return new ForgotPasswordRequest {
            Email = Email,
            CountryId = CountryId,
            PhoneNumber = PhoneNumber,
            Provider = (MFAProvider)Enum.Parse(typeof(MFAProvider), Provider),
            ReSend = Resent
        };
    }
}