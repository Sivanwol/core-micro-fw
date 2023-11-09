using Application.Responses.Base;
using Infrastructure.Enums;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Chat;

public class ChatReportUserRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int ReportedUserId { get; set; }
    public ReportFlag ReportType { get; set; }
    public string Reason { get; set; } = string.Empty;
}