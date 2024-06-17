using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "User Login"
})]
[SwaggerTag("Auth Controllers")]
public class LoginRequest {
    [SwaggerSchema("Email of the user")]
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [SwaggerSchema("Password of the user")]
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [SwaggerSchema("preserve login status")]
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    [SwaggerSchema("MFA Provide method")]
    [Required]
    public string Provider { get; set; }

    [Required]
    [SwaggerSchema("Resent OTP")]
    public bool Resent { get; set; } = false;

    public Processor.Services.User.LoginWebUserRequest ToProcessorEntity(string ipAddress, string userAgent) {
        return new Processor.Services.User.LoginWebUserRequest {
            Email = Email,
            Password = Password,
            RememberMe = RememberMe,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ReSend = Resent,
            Provider = Enum.Parse<MFAProvider>(Provider)
        };
    }
}