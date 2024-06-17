using Infrastructure.GQL;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class UpdateViewColumnsRequest : IRequest<View> {
    public string UserId { get; set; }
    public Guid ViewClientKey { get; set; }
    public IEnumerable<int> Columns { get; set; }
}