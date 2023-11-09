using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class RegisterUserUpdateFilesHandler : IRequestHandler<UpdateUserRegisterMediaRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public RegisterUserUpdateFilesHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(UpdateUserRegisterMediaRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RegisterUserUpdateFilesHandler: [{request.UserId}]");
        await _appUserRepository.UpdateUserPhotos(request);
        return await Task.FromResult(new EmptyResponse());
    }
}