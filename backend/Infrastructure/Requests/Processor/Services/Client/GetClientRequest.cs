using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class GetClientRequest : BaseRequest<GQL.Client?> {
    public int ClientId { get; set; }
}