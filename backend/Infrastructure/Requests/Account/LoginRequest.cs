using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class LoginRequest {
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
}