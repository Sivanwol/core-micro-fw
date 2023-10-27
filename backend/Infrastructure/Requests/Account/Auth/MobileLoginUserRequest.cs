using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Account.Auth;

[SwaggerSchema(Required = new[] {
    "Login User"
})]
[SwaggerTag("Account")]
public class MobileLoginUserRequest {
    [SwaggerSchema("Phone Number of the user")]
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Country Id of the user")]
    [Required]
    [Display(Name = "Country Id")]
    public string CountryId { get; set; }
    
    /// <summary>
    /// this property is the provider received from (SMS=0, Email=1)
    /// </summary>
    [Required]
    [SwaggerSchema("Provider of the MFA (SMS=0, Email=1)")]
    public AuthProvidersMFA Provider { get; set; }
    
    
}