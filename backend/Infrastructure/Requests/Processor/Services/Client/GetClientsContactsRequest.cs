using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class GetClientsContactsRequest : BaseRequest<EntityPage<GQL.ClientContact>> {
    public int ClientId { get; set; }
    public EntityPage PageControl { get; set; }
}