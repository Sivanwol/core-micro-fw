using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Vendor;

public class GetVendorsRequest : BaseRequest<EntityPage<GQL.Vendor>> {
    public int? ClientId { get; set; }
    public EntityPage PageControl { get; set; }
}