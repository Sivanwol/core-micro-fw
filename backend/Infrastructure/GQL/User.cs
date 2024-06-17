using System.ComponentModel;
namespace Infrastructure.GQL;

[Description("User entity")]
public class User {

    [Description("entity Id (it guid format)")]
    public string Id { get; set; }

    [Description("User email")]
    public string Email { get; set; }

    [Description("User first name")]
    public string FirstName { get; set; }

    [Description("User last name")]
    public string LastName { get; set; }

    [Description("User phone number (may not verify so wont be entered)")]
    public string? PhoneNumber { get; set; }

    [Description("User address (if no phone this field empty string)")]
    public string Address { get; set; }

    [Description("User country (may not enter as this tied to phone number)")]
    public Country? Country { get; set; }

    [Description("User display language on the ui (may not verify so wont be entered)")]
    public Language? DisplayLanguage { get; set; }

    [Description("what roles user have")]
    public IList<string> Roles { get; set; } = new List<string>();

    [Description("User Registration Date")]
    public DateTime RegisterCompletedAt { get; set; }

    [Description("User Registration Date")]
    public IList<UserPreference> Preferences { get; set; } = new List<UserPreference>();
}

public class UserPreference {
    public string Key { get; set; }
    public string Value { get; set; }
}