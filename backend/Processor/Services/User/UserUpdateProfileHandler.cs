using Application.Exceptions;
using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class UserUpdateProfileHandler : IRequestHandler<UserUpdateProfileRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public UserUpdateProfileHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(UserUpdateProfileRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RegisterUserUpdateInfoHandler: [{request.UserId}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            Log.Logger.Error($"RegisterUserUpdateInfoHandler: User not found [{request.UserId}]");
            throw new EntityNotFoundException("User", request.UserId);
        }
        await _appUserRepository.UpdateUserProfile(request);

        return await Task.FromResult(new EmptyResponse());
    }
}