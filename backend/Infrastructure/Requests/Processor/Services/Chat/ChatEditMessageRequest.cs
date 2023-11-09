using Application.Responses.Base;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Chat;

public class ChatEditMessageRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int MessageId { get; set; }
    public string Message { get; set; }
}