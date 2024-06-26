using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Services.Auth.Models;
namespace Infrastructure.Services.Auth;

public interface IJwtAuthManager {
    IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
    JwtAuthResult GenerateTokens(string userToken, IList<Claim> claims, DateTime now);
    JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
    void RemoveExpiredRefreshTokens(DateTime now);
    void RemoveRefreshTokenByUserToken(string userToken);
    (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
}