using Infrastructure.GQL;
using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class UserActivityRequest : BaseRequest<EntityPage<Activities>> {
    public Guid UserId { get; set; }
    public EntityPage PageControl { get; set; }
}