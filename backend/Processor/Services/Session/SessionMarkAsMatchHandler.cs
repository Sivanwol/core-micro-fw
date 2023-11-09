using Application.Exceptions;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Session;
using Infrastructure.Responses.Controllers.Session;
using MediatR;
using Serilog;
namespace Processor.Services.Session;

public class SessionMarkAsMatchHandler : IRequestHandler<SessionMarkAsMatchRequest, SessionInfoResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;
    private readonly ISessionRepository _sessionRepository;

    public SessionMarkAsMatchHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        ISessionRepository sessionRepository) {
        _appUserRepository = appUserRepository;
        _sessionRepository = sessionRepository;
        _mediator = mediator;
    }


    public async Task<SessionInfoResponse> Handle(SessionMarkAsMatchRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"SessionMarkAsMatchHandler: [{request.SessionId}-{request.UserId}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var sessionExist = await _appUserRepository.HasUserSessionExist(request.SessionId, request.UserId);
        if (!sessionExist) {
            throw new EntityNotFoundException("Session", $"[{request.SessionId}-{request.UserId}]");
        }
        var result = await _sessionRepository.MarkMatchSession(request.SessionId, request.UserId);
        return await Task.FromResult(new SessionInfoResponse {
            Status = Enum.Parse<SessionStatus>(result.Status),
            FeedbackRating = result.Session.FeedbackRating,
            Users = result.MatchUserIds,
            CurrentQuestion = result.CurrentQuestion.ToResponse()
        });
    }
}