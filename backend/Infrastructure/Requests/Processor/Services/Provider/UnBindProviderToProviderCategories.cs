using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;

namespace Infrastructure.Requests.Processor.Services.Provider;

public class UnBindProviderToProviderCategories: BaseRequest<EmptyResponse>
{
    public IEnumerable<int> CategoryIds { get; set; }
    public int ProviderId { get; set; }
}
