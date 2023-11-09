using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class ExportUserHandler : IRequestHandler<DeleteUserRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public ExportUserHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"ExportUserHandler: {request.UserId}");
        await _appUserRepository.ExportUser(request.UserId);
        return await Task.FromResult(new EmptyResponse());
    }
}