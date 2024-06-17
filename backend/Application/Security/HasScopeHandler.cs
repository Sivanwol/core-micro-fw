using Microsoft.AspNetCore.Authorization;
namespace Application.Security;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement> {
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasScopeRequirement requirement
    ) {
        // If user does not have the scope claim, get out of here
        if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
            return Task.CompletedTask;

        // Split the permissions string into an array
        var permissions = context.User
            .FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer);

        // Succeed if the scope array contains the required scope
        if (permissions.Any(s => s.Value == requirement.Scope)) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}