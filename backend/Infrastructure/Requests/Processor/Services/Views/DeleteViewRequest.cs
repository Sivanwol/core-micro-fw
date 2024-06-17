using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class DeleteViewRequest : IRequest<bool> {
    public string UserId { get; set; }
    public Guid ViewClientKey { get; set; }
}