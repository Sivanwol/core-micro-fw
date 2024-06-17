using System.ComponentModel;
namespace Infrastructure.GQL.Inputs.User;

[Description("Update my user preference")]
public class UserPreferenceInput {
    [Description("Preference Key")]
    public string Key { get; set; }

    [Description("Preference Value")]
    public string Value { get; set; }
}