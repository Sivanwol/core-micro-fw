using Infrastructure.Responses.Controllers.Session;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Session;

public class SessionAnswerQuestionRequest : IRequest<SessionInfoResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int QuestionId { get; set; }
    public int? AnswerId { get; set; }
    public string? AnswerText { get; set; }
}