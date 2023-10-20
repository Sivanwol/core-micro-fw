using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Requests.Account;

public class ResetPasswordRequest {
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required] public string Code { get; set; }
}