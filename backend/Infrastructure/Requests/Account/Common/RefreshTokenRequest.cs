using System.Text.Json.Serialization;
namespace Infrastructure.Requests.Backoffice.Account;

public class RefreshTokenRequest {
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; }
}