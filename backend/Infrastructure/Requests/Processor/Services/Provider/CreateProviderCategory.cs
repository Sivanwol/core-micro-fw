using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;

namespace Infrastructure.Requests.Processor.Services.Provider;

public class CreateProviderCategory: BaseRequest<GQL.ProviderCategory>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";

}
