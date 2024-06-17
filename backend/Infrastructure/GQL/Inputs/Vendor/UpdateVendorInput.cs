using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Vendor;

namespace Infrastructure.GQL.Inputs.Vendor;

[Description("Update new vendor")]
public class UpdateVendorInput : CreateVendorInput
{
    [Description("Vendor Id")]
    public int Id { get; set; }

    public UpdateVendorRequest ToProcessorEntity(Guid loggedInUserId, string ipAddress, string userAgent)
    {
        return new UpdateVendorRequest
        {
            VendorId = Id,
            Name = Name,
            Description = Description,
            CountryId = CountryId,
            SiteUrl = SiteUrl,
            City = City,
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
    public UpdateClientVendorRequest ToProcessorEntity(int clientId, Guid loggedInUserId, string ipAddress, string userAgent)
    {
        return new UpdateClientVendorRequest
        {
            ClientId = clientId,
            Name = Name,
            Description = Description,
            CountryId = CountryId,
            SiteUrl = SiteUrl,
            City = City,
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
}
