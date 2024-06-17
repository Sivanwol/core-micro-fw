using Infrastructure.GQL;
using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.ActivitiesLogs;

public class GetActivityRequest : BaseRequest<EntityPage<Activities>> {
    public EntityPage PageControl { get; set; }
}