using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class ApplicationUserRole : IdentityUserRole<string> {
    public virtual ApplicationUser User { get; set; }
    public virtual AspNetRoles Role { get; set; }
}