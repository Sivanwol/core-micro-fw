using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Vendor;

public class GetVendorRequest : BaseRequest<GQL.Vendor?>
{
    public int VendorId { get; set; }
}