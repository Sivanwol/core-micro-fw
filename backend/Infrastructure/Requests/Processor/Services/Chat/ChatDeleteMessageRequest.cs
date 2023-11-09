using Application.Responses.Base;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Chat;

public class ChatDeleteMessageRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int MessageId { get; set; }
}