using Infrastructure.Responses.Common;
using Infrastructure.Services.Auth.Models;

namespace Infrastructure.Responses.Auth;

public class AuthResponse : DataResponse<AuthResponse> {
    /// <summary>
    /// This property hold the jwt tokens
    /// </summary>
    public JwtAuthResult Tokens { get; set; }

    /// <summary>
    /// user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// in case of user required do MFA action this property will be true
    /// </summary>
    public bool RequeiredMFA { get; set; }

    public bool LockedOut { get; set; }
}