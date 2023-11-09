using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Backoffice;

[SwaggerSchema(Required = new[] {
    "Verify MFA Code"
})]
[SwaggerTag("Auth Controllers")]
public class VerifyFromProviderRequest {
    /// <summary>
    /// this property is the provider received from (SMS=0, Email=1)
    /// </summary>
    [Required]
    [SwaggerSchema("Provider of the MFA (SMS=0, Email=1)")]
    public MFAProvider Provider { get; set; }

    /// <summary>
    /// this property code that user get it from provider
    /// </summary>
    [Required]
    [SwaggerSchema("Code of the MFA")]
    public string Code { get; set; }

    /// <summary>
    /// this property if user want to remember the login if he use 2FA Via browser ignore this property if not
    /// </summary>
    [Display(Name = "Remember me?")]
    [Required]
    [SwaggerSchema("preserve login status")]
    public bool RememberMe { get; set; }
}