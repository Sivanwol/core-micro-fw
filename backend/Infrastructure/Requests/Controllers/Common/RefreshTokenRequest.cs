using System.Text.Json.Serialization;
namespace Infrastructure.Requests.Controllers.Common;

public class RefreshTokenRequest {
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; }
}