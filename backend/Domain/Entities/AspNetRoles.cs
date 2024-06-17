using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class AspNetRoles : IdentityRole {
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}