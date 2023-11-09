using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Backoffice;

[SwaggerSchema(Required = new[] {
    "Forget User Password"
})]
[SwaggerTag("Auth Controllers")]
public class ForgetPasswordRequest {

    [SwaggerSchema("Email of the user")]
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}