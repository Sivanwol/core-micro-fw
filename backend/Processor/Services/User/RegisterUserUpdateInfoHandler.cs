using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class RegisterUserUpdateInfoHandler : IRequestHandler<NewUserRegisterInfoRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public RegisterUserUpdateInfoHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(NewUserRegisterInfoRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RegisterUserUpdateInfoHandler: [{request.UserId}]");
        await _appUserRepository.NewUserProfileInfo(request);
        return await Task.FromResult(new EmptyResponse());
    }
}