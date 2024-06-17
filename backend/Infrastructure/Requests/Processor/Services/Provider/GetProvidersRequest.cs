using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;

namespace Infrastructure.Requests.Processor.Services.Provider;

public class GetProvidersRequest : BaseRequest<EntityPage<GQL.Provider>>
{
    public int? ClientId { get; set; }
    public EntityPage PageControl { get; set; }
}