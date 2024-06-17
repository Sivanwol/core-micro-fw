using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class GetClientsRequest : BaseRequest<EntityPage<GQL.Client>> {
    public EntityPage PageControl { get; set; }
}