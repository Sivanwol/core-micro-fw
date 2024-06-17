using Application.Responses.Base;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class ResetPasswordRequest : IRequest<EmptyResponse> {
    public Guid LoggedUserId { get; set; }
    public Guid UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}