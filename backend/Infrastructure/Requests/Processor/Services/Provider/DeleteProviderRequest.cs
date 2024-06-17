using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;

namespace Infrastructure.Requests.Processor.Services.Provider;

public class DeleteProviderRequest: BaseRequest<EmptyResponse>
{
    public int ProviderId { get; set; }
}
