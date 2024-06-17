using Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Domain.Utils;

public static class AuthUtils {

    public static async Task<bool> HasMathchingRoles(UserManager<ApplicationUser> userManager, ApplicationUser user, List<string> roles) {
        if (roles.Count == 0) {
            return true;
        }
        var userRoles = await userManager.GetRolesAsync(user);
        return userRoles.Any(r => roles.Any(role => role == r));
    }
}