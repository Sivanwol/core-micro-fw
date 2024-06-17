using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class GetUsersRequest : BaseRequest<EntityPage<GQL.User>> {
    public EntityPage PageControl { get; set; }
}