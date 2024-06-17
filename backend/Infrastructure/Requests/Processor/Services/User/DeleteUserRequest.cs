using Application.Responses.Base;
using Infrastructure.Requests.Processor.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class DeleteUserRequest : BaseRequest<EmptyResponse> {
    public Guid UserId { get; set; }
}