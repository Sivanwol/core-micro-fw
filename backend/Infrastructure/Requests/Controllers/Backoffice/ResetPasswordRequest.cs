using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Backoffice;

[SwaggerSchema(Required = new[] {
    "Reset User Password"
})]
[SwaggerTag("Auth Controllers")]
public class ResetPasswordRequest {
    [SwaggerSchema("Email of the user")]
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [SwaggerSchema("old password of the user")]
    [Required]
    [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [SwaggerSchema("new password of the user")]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }


    [SwaggerSchema("Security token of the user")]
    [Required]
    public string Token { get; set; }
}