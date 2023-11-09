using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Backoffice;

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
}