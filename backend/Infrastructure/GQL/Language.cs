using System.ComponentModel;
namespace Infrastructure.GQL;

[Description("Language entity")]
public class Language {

    [Description("entity id")]
    public int Id { get; set; }

    [Description("language name")]
    public string Name { get; set; }

    [Description("language code per iso")]
    public string Code { get; set; }

    [Description("what asset (on the frontend) it link to basiclly the flag")]
    public string Flag { get; set; }
}