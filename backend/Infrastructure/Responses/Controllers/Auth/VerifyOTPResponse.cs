using Infrastructure.Services.Auth.Models;
namespace Infrastructure.Responses.Controllers.Auth;

public class VerifyOTPResponse {
    /// <summary>
    ///     This property hold the jwt tokens
    /// </summary>
    public JwtAuthResult? Tokens { get; set; }

    /// <summary>
    ///     user id
    /// </summary>
    public Guid? UserId { get; set; }
}