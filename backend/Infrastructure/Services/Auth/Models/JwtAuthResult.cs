using System.Text.Json.Serialization;

namespace Infrastructure.Services.Auth.Models;

public class JwtAuthResult {
    /// <summary>
    /// the access token issued by the authentication server
    /// </summary>
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    /// <summary>
    /// ;the refresh token object issued by the authentication server
    /// </summary>
    [JsonPropertyName("refreshToken")]
    public RefreshToken RefreshToken { get; set; }
}