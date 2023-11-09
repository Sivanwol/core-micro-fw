using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public DeleteUserHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"DeleteUserHandler: {request.UserId}");
        await _appUserRepository.DeleteUser(request.UserId);
        return await Task.FromResult(new EmptyResponse());
    }
}