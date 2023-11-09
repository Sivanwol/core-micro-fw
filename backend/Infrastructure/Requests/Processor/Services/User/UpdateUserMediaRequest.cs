using Application.Responses.Base;
using Infrastructure.Requests.Processor.Services.Common;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class UpdateUserMediaRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public IEnumerable<FileDetail> Files { get; set; }
    public IEnumerable<int> FilesToDelete { get; set; }
}