using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class VerifyViewExistRequest : IRequest<bool> {
    public string UserId { get; set; }
    public Guid ViewClientKey { get; set; }
    public string Name { get; set; }
}