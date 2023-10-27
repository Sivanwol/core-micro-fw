using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Account.Backoffice;

[SwaggerSchema(Required = new[] {
    "Forget User Password"
})]
[SwaggerTag("Backoffice Account")]
public class ForgetPasswordRequest {
    
    [SwaggerSchema("Email of the user")]
    [Required] 
    [EmailAddress] 
    public string Email { get; set; }
}