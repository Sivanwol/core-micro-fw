using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;

namespace Infrastructure.Models.Account;

public class VerifyFromProviderRequest {
    /// <summary>
    /// this property is the provider received from (SMS=0, Email=1)
    /// </summary>
    public AuthProvidersMFA Provider { get; set; }

    /// <summary>
    /// this property code that user get it from provider
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// this property if user want to remember the login if he use 2FA Via browser ignore this property if not
    /// </summary>
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}