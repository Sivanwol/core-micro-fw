using System.ComponentModel.DataAnnotations;
using Infrastructure.Requests.Processor.Services.User;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

[SwaggerSchema(Required = new[] {
    "Register New User"
})]
[SwaggerTag("Auth Controllers")]
public class RegisterNewUserRequest {
    [SwaggerSchema("Email of the user")]
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [SwaggerSchema("Password of the user (at least 6 characters)")]
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [SwaggerSchema("First name of the user")]
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [SwaggerSchema("Last name of the user")]
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [SwaggerSchema("Phone number of the user")]
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Country Id of the user")]
    [Required]
    [Display(Name = "Country Id")]
    public string CountryId { get; set; }

    [SwaggerSchema("Display Language Id of the user")]
    [Required]
    [Display(Name = "Display Language Id")]
    public string DisplayLanguageId { get; set; }

    public RegisterRequest ToProcessorEntity(string ipAddres, string userAgent) => new() {
        Email = Email,
        Password = Password,
        FirstName = FirstName,
        LastName = LastName,
        PhoneNumber = PhoneNumber,
        CountryId = int.Parse(CountryId),
        DisplayLanguageId = int.Parse(DisplayLanguageId),
        IpAddress = ipAddres,
        UserAgent = userAgent
    };
}