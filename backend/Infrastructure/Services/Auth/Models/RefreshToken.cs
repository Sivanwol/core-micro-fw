using System.Text.Json.Serialization;

namespace Infrastructure.Services.Auth.Models;

public class RefreshToken {
    /// <summary>
    /// user name (email) of the account that the token is for
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } // can be used for usage tracking
    // can optionally include other metadata, such as user agent, ip address, device name, and so on

    /// <summary>
    /// the new token (to replace the old one)
    /// </summary>
    [JsonPropertyName("tokenString")]
    public string TokenString { get; set; }

    /// <summary>
    /// when this token expires
    /// </summary>
    [JsonPropertyName("expireAt")]
    public DateTime ExpireAt { get; set; }
}