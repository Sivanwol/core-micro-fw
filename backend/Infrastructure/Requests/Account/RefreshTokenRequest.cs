using System.Text.Json.Serialization;

namespace Infrastructure.Models.Account;

public class RefreshTokenRequest {
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; }
}