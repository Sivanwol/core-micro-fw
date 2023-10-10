using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser {
    [ProtectedPersonalData] public Countries Country { get; set; }
    [PersonalData] public string FirstName { get; set; }
    [PersonalData] public string LastName { get; set; }
}