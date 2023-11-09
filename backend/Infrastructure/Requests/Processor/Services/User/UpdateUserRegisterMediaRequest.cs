using Application.Responses.Base;
using Infrastructure.Requests.Processor.Services.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class UpdateUserRegisterMediaRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public IEnumerable<FileDetail> Files { get; set; }
}