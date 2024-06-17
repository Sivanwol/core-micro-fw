
using System.ComponentModel;

namespace Infrastructure.GQL;

[Description("Provider Category Object")]
public class ProviderCategory
{

    [Description("Provider Category Id")]
    public int Id { get; set; }
    [Description("Provider Category Name")]
    public string Name { get; set; }
    [Description("Provider Category Description")]
    public string Description { get; set; } = "";
}