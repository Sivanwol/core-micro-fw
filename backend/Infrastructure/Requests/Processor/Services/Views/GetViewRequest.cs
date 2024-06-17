using Infrastructure.GQL;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class GetViewRequest : IRequest<View> {
    public string UserId { get; set; }
    public Guid ViewClientKey { get; set; }
}