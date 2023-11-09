using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Services.Auth;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class LogoutUserHandler : IRequestHandler<LogoutUserRequest, bool> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;

    public LogoutUserHandler(IMediator mediator,
        IJwtAuthManager jwtAuthManager,
        IAppUserRepository appUserRepository,
        IOTPRepository otpRepository) {
        _otpRepository = otpRepository;
        _jwtAuthManager = jwtAuthManager;
        _mediator = mediator;
        _appUserRepository = appUserRepository;
    }

    public async Task<bool> Handle(LogoutUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"LogoutUserHandler: [{request.UserToken}]");
        var result = await _otpRepository.LocateUserByToken(request.UserToken);
        if (!result)
            return false;
        _jwtAuthManager.RemoveRefreshTokenByUserToken(request.UserToken);
        return true;
    }
}