using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class GetClientsContactRequest : BaseRequest<GQL.ClientContact?> {
    public int ClientId { get; set; }
    public int ClientContactId { get; set; }
}