using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Requests.Account;

public class ForgetPasswordRequest {
    [Required] [EmailAddress] public string Email { get; set; }
}