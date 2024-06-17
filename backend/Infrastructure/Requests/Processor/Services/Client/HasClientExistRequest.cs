using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class HasClientExistRequest : BaseRequest<bool>  {
    public int CountryId { get; set; }
    public string Name { get; set; }
}