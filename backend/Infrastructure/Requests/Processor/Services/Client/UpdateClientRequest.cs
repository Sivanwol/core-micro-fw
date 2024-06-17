using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class UpdateClientRequest : BaseRequest<EmptyResponse> {

    public int ClientId { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Website { get; set; }
    public int CountryId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}