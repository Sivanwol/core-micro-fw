using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Services.Auth.Models;

[SwaggerSchema(Required = new[] {
    "JWT Token Response"
})]
[SwaggerTag("Common")]
public class JwtAuthResult {


    [SwaggerSchema("JWT Access Token")]
    public string AccessToken { get; set; }

    [SwaggerSchema("JWT Refresh Token")]
    public RefreshToken RefreshToken { get; set; }
}