using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Request OTP for user"
})]
[SwaggerTag("Controllers")]
public class VerifyOTPUserRequest {
    [Required]
    [SwaggerSchema("Code of the MFA")]
    public string Code { get; set; }

    [Required]
    [SwaggerSchema("OTP Token of the MFA")]
    public string OTPToken { get; set; }

    [Required]
    [SwaggerSchema("User Token")]
    public string UserToken { get; set; }
}