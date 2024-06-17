using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Requests.Processor.Services.Vendor;

public class CreateVendorRequest : BaseRequest<Infrastructure.GQL.Vendor>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CountryId { get; set; }
    public IFormFile Logo { get; set; }
    public string SiteUrl { get; set; }
    public string SupportUrl { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? SupportPhone { get; set; }
    public string? SupportEmail { get; set; }
    public VendorSupportResponseType SupportResponseType { get; set; }
}
