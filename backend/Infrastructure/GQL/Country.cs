using System.ComponentModel;
namespace Infrastructure.GQL;

[Description("Country entity")]
public class Country {
    [Description("entity id")]
    public int Id { get; set; }

    [Description("coutnry name")]
    public string CountryName { get; set; }

    [Description("coutnry code (us,il and so on)")]
    public string CountryCode { get; set; }

    [Description("country phone ext like (+1, +972) with the +")]
    public string CountryNumber { get; set; }
}