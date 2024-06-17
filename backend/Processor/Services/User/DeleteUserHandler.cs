using Application.Responses.Base;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, EmptyResponse> {
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IMediator _mediator;

    public DeleteUserHandler(IMediator mediator, IApplicationUserRepository applicationUserRepository) {
        _applicationUserRepository = applicationUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"DeleteUserHandler: {request.UserId}");
        await _applicationUserRepository.DeleteUser(request.UserId);
        return await Task.FromResult(new EmptyResponse());
    }
}