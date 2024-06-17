using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Enums;

namespace Infrastructure.GQL;

[Description("Vendor Object")]
public class Vendor
{
    [Description("Vendor Id")]
    public int Id { get; set; }
    [Description("Vendor Name")]
    public string Name { get; set; }
    [Description("Vendor decription")]
    public string Description { get; set; }
    [Description("Vendor country")]
    public Country Country { get; set; }
    [Description("Vendor logo (Media object)")]
    public Media? Logo { get; set; }
    [Description("Vendor url")]
    public string SiteUrl { get; set; }
    [Description("Vendor support url")]
    public string SupportUrl { get; set; }
    [Description("Vendor address")]
    public string Address { get; set; }
    [Description("Vendor support phone")]
    public string? SupportPhone { get; set; }
    [Description("Vendor support email")]
    public string? SupportEmail { get; set; }
    [Description("Vendor support response type (for automation)")]
    public VendorSupportResponseType SupportResponseType { get; set; }
    [Description("Vendor list on contacts")]
    public IEnumerable<Contact> Contacts { get; set; }
    [Description("Vendor disabled at")]
    public DateTime? DisabledAt { get; set; }

}
