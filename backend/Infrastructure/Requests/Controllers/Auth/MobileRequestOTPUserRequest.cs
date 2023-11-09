using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Request OTP for user"
})]
[SwaggerTag("Controllers")]
public class MobileRequestOTPUserRequest {
    [Required]
    [SwaggerSchema("Phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    [SwaggerSchema("Country Id")]
    public int CountryId { get; set; }

    [Required]
    [SwaggerSchema("Provider of the MFA")]
    public string Provider { get; set; }
}