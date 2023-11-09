using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class RegisterUserUpdatePreferenceHandler : IRequestHandler<NewUserRegisterPreferenceRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public RegisterUserUpdatePreferenceHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(NewUserRegisterPreferenceRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RegisterUserUpdateInfoHandler: [{request.UserId}]");
        await _appUserRepository.NewUserProfilePreference(request);
        return await Task.FromResult(new EmptyResponse());
    }
}