using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Enums;

namespace Infrastructure.GQL;

[Description("Provider Object")]
public class Provider
{
    [Description("Provider Id")]
    public int Id { get; set; }
    [Description("Provider Name")]
    public string Name { get; set; }
    [Description("Provider decription")]
    public string Description { get; set; }
    [Description("Provider country")]
    public Country Country { get; set; }
    [Description("Provider logo (Media object)")]
    public Media Logo { get; set; }
    [Description("Provider url")]
    public string SiteUrl { get; set; }
    [Description("Provider support url")]
    public string SupportUrl { get; set; }
    [Description("Provider address")]
    public string Address { get; set; }
    [Description("Provider support phone")]
    public string? SupportPhone { get; set; }
    [Description("Provider support email")]
    public string? SupportEmail { get; set; }
    [Description("Provider support response type (for automation)")]
    public ProviderType ProviderType { get; set; }
    [Description("Provider list on contacts")]
    public IEnumerable<Contact> Contacts { get; set; }
    [Description("Provider disabled at")]
    public DateTime? DisabledAt { get; set; }

}
