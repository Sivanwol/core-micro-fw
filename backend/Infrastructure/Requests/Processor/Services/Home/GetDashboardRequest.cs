using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Home;

public class GetDashboardRequest : BaseRequest<Dashboard> {
}