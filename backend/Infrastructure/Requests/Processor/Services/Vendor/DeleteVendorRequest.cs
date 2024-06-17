using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;

namespace Infrastructure.Requests.Processor.Services.Vendor;

public class DeleteVendorRequest: BaseRequest<EmptyResponse>
{
    public int VendorId { get; set; }
}
