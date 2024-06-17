using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Vendor;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.GQL.Inputs.Vendor;

[Description("Create new vendor")]
public class CreateVendorInput
{
    [Description("Vendor Name")]
    public string Name { get; set; }
    [Description("Vendor decription")]
    public string Description { get; set; }
    [Description("Vendor country id")]
    public int CountryId { get; set; }
    [Description("Vendor logo id (Media object id)")]
    public string SiteUrl { get; set; }
    [Description("Vendor support url")]
    public string SupportUrl { get; set; }
    [Description("Vendor city")]
    public string City { get; set; }
    [Description("Vendor address")]
    public string Address { get; set; }
    [Description("Vendor support phone")]
    public string? SupportPhone { get; set; }
    [Description("Vendor support email")]
    public string? SupportEmail { get; set; }
    [Description("Vendor support response type (for automation)")]
    public VendorSupportResponseType SupportResponseType { get; set; }

    public CreateVendorRequest ToProcessorEntity(IFormFile logo, Guid loggedInUserId, string ipAddress, string userAgent)
    {
        return new CreateVendorRequest
        {
            Name = Name,
            Description = Description,
            CountryId = CountryId,
            SiteUrl = SiteUrl,
            City = City,
            Logo = logo,
            Address = Address,
            SupportUrl = SupportUrl,
            SupportEmail = SupportEmail,
            SupportPhone = SupportPhone,
            SupportResponseType = SupportResponseType,
            LoggedInUserId = loggedInUserId,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }

    public CreateClientVendorRequest ToProcessorEntity(int clientId, IFormFile logo, Guid loggedInUserId, string ipAddress, string userAgent)
    {
        return new CreateClientVendorRequest
        {
            ClientId = clientId,
            Name = Name,
            Description = Description,
            CountryId = CountryId,
            SiteUrl = SiteUrl,
            City = City,
            Address = Address,
            Logo = logo,
            SupportUrl = SupportUrl,
            SupportEmail = SupportEmail,
            SupportPhone = SupportPhone,
            SupportResponseType = SupportResponseType,
            LoggedInUserId = loggedInUserId,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }
}
