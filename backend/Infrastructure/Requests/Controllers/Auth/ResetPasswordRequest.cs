using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Reset User Password"
})]
[SwaggerTag("Auth Controllers")]
public class ResetPasswordRequest {
    [SwaggerSchema("User Id")]
    [Required]
    public string UserId { get; set; }

    [SwaggerSchema("old password of the user")]
    [Required]
    [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [SwaggerSchema("new password of the user")]
    [DataType(DataType.Password)]
    [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    public Processor.Services.User.ResetPasswordRequest ToProcessorEntity(Guid loggedUserId) {
        return new Processor.Services.User.ResetPasswordRequest {
            LoggedUserId = loggedUserId,
            UserId = Guid.Parse(UserId),
            OldPassword = OldPassword,
            NewPassword = NewPassword
        };
    }
}