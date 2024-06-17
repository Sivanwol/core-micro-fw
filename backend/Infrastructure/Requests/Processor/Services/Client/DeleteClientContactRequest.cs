using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Client;

public class DeleteClientContactRequest :  BaseRequest<EmptyResponse> {
    public int ClientId { get; set; }
    public int ClientContactId { get; set; }
}