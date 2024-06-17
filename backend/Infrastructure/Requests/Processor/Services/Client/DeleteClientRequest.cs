using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class DeleteClientRequest : IRequest {
    public Guid LoggedInUserId { get; set; }
    public int ClientId { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
}