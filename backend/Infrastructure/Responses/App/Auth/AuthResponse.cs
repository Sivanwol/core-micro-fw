using Infrastructure.Services.Auth.Models;
namespace Infrastructure.Responses.App.Auth; 

public class AuthResponse {
    /// <summary>
    /// This property hold the jwt tokens
    /// </summary>
    public JwtAuthResult? Tokens { get; set; }

    /// <summary>
    /// user id
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// User Token
    /// </summary>
    public string UserToken { get; set; }

    /// <summary>
    /// in case of user required do MFA action this property will be true
    /// </summary>
    public bool RequeiredMFA { get; set; }

    /// <summary>
    /// Is User Locked Out
    /// </summary>
    public bool LockedOut { get; set; }
}