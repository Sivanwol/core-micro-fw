using Application.Responses.Base;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Session;

public class SessionReplyToQuestionRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int QuestionId { get; set; }
    public string Message { get; set; }
}