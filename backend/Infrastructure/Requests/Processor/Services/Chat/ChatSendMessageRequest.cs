using Application.Responses.Base;
using Infrastructure.Enums;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Chat;

public class ChatSendMessageRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int? ReplyId { get; set; }
    public string Message { get; set; }
    public MessageType Type { get; set; } = MessageType.Message;
}