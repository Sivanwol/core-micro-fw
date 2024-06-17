using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Backend.api.Factories;

public class AppClaimsFactory : IUserClaimsPrincipalFactory<ApplicationUser> {
    public Task<ClaimsPrincipal> CreateAsync(ApplicationUser user) {

        var claims = new Claim[] {
            new Claim(ClaimTypes.Email, user.Email ?? ""), new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"), new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Authentication, "true"), new Claim("TenantId", ""), // <--- Here
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Bearer");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return Task.FromResult(claimsPrincipal);

    }
}