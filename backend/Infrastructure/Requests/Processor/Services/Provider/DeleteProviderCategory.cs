using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;

namespace Infrastructure.Requests.Processor.Services.Provider;

public class DeleteProviderCategory: BaseRequest<EmptyResponse>
{
    public int CategoryId { get; set; }
}
