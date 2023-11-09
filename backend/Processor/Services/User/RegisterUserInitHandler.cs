using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class RegisterUserInitHandler : IRequestHandler<NewUserInitRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public RegisterUserInitHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<EmptyResponse> Handle(NewUserInitRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetUserProfileHandler: [{request.CountryId},{request.PhoneNumber}]");
        await _appUserRepository.RegisterUserInit(request);
        return await Task.FromResult(new EmptyResponse());
    }
}