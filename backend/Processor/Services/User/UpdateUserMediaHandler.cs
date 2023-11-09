using Application.Exceptions;
using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
namespace Processor.Services.User;

public class UpdateUserMediaHandler : IRequestHandler<UpdateUserMediaRequest, EmptyResponse> {

    private readonly IAppUserRepository _appUserRepository;
    private readonly ILogger _logger;
    private readonly IMediaRepository _mediaRepository;
    private readonly IMediator _mediator;

    public UpdateUserMediaHandler(IMediator mediator, IAppUserRepository appUserRepository, IMediaRepository mediaRepository, ILoggerFactory logFactory) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
        _mediaRepository = mediaRepository;
        _logger = logFactory.CreateLogger<UpdateUserMediaHandler>();
    }

    public async Task<EmptyResponse> Handle(UpdateUserMediaRequest request, CancellationToken cancellationToken) {
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            _logger.LogWarning($"UpdateUserMediaHandler: User not found [{request.UserId}]");
            throw new EntityNotFoundException("User", request.UserId);
        }
        if (request.FilesToDelete.Any()) {
            await _mediaRepository.DeleteUserSelectedMedia(request.UserId, request.FilesToDelete);
        }
        return await Task.FromResult(new EmptyResponse());
    }
}