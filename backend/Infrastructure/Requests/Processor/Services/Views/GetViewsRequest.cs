using Infrastructure.GQL;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class GetViewsRequest : IRequest<IEnumerable<View>> {
    public string UserId { get; set; }
}