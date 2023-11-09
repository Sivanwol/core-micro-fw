using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.Services.Auth.Models;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructure.Services.Auth;

public class JwtAuthManager : IJwtAuthManager {

    private readonly JwtTokenConfig _jwtTokenConfig;
    private readonly byte[] _secret;

    private readonly ConcurrentDictionary<string, RefreshToken>
        _usersRefreshTokens; // can store in a database or a distributed cache

    public JwtAuthManager(JwtTokenConfig jwtTokenConfig) {
        _jwtTokenConfig = jwtTokenConfig;
        _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
        _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
    }

    public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary =>
        _usersRefreshTokens.ToImmutableDictionary();

    // optional: clean up expired refresh tokens
    public void RemoveExpiredRefreshTokens(DateTime now) {
        var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
        foreach (var expiredToken in expiredTokens) {
            _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
        }
    }

    // can be more specific to ip, user agent, device name, etc.
    public void RemoveRefreshTokenByUserToken(string userToken) {
        var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserToken == userToken).ToList();
        foreach (var refreshToken in refreshTokens) {
            _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
        }
    }

    public JwtAuthResult GenerateTokens(string userToken, IList<Claim> claims, DateTime now) {
        var shouldAddAudienceClaim =
            string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
        var jwtToken = new JwtSecurityToken(
            _jwtTokenConfig.Issuer,
            shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
            claims,
            expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        var refreshToken = new RefreshToken {
            UserToken = userToken,
            TokenString = GenerateRefreshTokenString(),
            ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
        };
        _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (_, _) => refreshToken);

        return new JwtAuthResult {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now) {
        var (principal, jwtToken) = DecodeJwtToken(accessToken);
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature)) {
            throw new SecurityTokenException("Invalid token");
        }

        var userToken = principal.Identity?.Name;
        if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken)) {
            throw new SecurityTokenException("Invalid token");
        }

        if (existingRefreshToken.UserToken != userToken || existingRefreshToken.ExpireAt < now) {
            throw new SecurityTokenException("Invalid token");
        }

        return GenerateTokens(userToken, principal.Claims.ToArray(), now); // need to recover the original claims
    }

    public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token) {
        if (string.IsNullOrWhiteSpace(token)) {
            throw new SecurityTokenException("Invalid token");
        }

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidAudience = _jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                },
                out var validatedToken);
        return (principal, validatedToken as JwtSecurityToken);
    }

    private static string GenerateRefreshTokenString() {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}