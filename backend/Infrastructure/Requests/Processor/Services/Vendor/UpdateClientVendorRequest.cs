using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Common;
using MediatR;

namespace Infrastructure.Requests.Processor.Services.Vendor;

public class UpdateClientVendorRequest: UpdateVendorRequest
{
    public int ClientId { get; set; }   
}
