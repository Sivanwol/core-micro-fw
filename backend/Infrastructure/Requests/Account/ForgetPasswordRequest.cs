using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class ForgetPasswordRequest {
    [Required] [EmailAddress] public string Email { get; set; }
}