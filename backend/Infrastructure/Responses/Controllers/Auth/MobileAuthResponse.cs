using Infrastructure.Services.Auth.Models;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Responses.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Auth Response "
})]
[SwaggerTag("Controllers")]
public class MobileAuthResponse {

    [SwaggerSchema("JWT Tokens")]
    public JwtAuthResult? Tokens { get; set; }

    [SwaggerSchema("user id")]
    public string UserId { get; set; }

    [SwaggerSchema("user token this is a unique token for the user")]
    public string UserToken { get; set; }
}