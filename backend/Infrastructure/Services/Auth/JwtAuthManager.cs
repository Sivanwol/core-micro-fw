using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Infrastructure.Services.Auth.Models;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructure.Services.Auth;

public class JwtAuthManager : IJwtAuthManager {

    private readonly JwtTokenConfig _jwtTokenConfig;

    private readonly ConcurrentDictionary<string, RefreshToken>
        _usersRefreshTokens; // can store in a database or a distributed cache

    public JwtAuthManager(JwtTokenConfig jwtTokenConfig) {
        _jwtTokenConfig = jwtTokenConfig;
        _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
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
        var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtTokenConfig.Secret));

        var accessToken = new JwtSecurityTokenHandler().CreateEncodedJwt(new SecurityTokenDescriptor {
            // Issuer = _jwtTokenConfig.Issuer,
            // Audience = _jwtTokenConfig.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature)
        });
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
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512)) {
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
                    // ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtTokenConfig.Secret)),
                    // ValidAudience = _jwtTokenConfig.Audience,
                    // ValidIssuer = _jwtTokenConfig.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // ClockSkew = TimeSpan.Zero
                    // 
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