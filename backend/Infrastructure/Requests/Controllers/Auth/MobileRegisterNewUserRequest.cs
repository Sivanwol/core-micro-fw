using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Register new user initiated by mobile stage 1: initial request"
})]
[SwaggerTag("Controllers")]
public class MobileRegisterNewUserRequest {
    [Required]
    [SwaggerSchema("Email")]
    public string Email { get; set; }

    [Required]
    [SwaggerSchema("First Name")]
    public string FirstName { get; set; }

    [Required]
    [SwaggerSchema("Last Name")]
    public string LastName { get; set; }

    [Required]
    [SwaggerSchema("Latitude")]
    public double Latitude { get; set; }

    [Required]
    [SwaggerSchema("Longitude")]
    public double Longitude { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [SwaggerSchema("Phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    [SwaggerSchema("Country Id")]
    public int CountryId { get; set; }

    [Required]
    [SwaggerSchema("Language Id")]
    public int LanguageId { get; set; }

    [Required]
    [SwaggerSchema("Birth Date")]
    public DateTime BirthDate { get; set; }
}