using Application.Exceptions;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Session;
using Infrastructure.Responses.Controllers.Session;
using MediatR;
using Serilog;
namespace Processor.Services.Session;

public class StartSessionHandler : IRequestHandler<StartSessionRequest, SessionInfoResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public StartSessionHandler(IMediator mediator,
        IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }


    public async Task<SessionInfoResponse> Handle(StartSessionRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"StartSessionHandler: {request.UserId}");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var result = await _appUserRepository.StartSession(request.UserId, request.MatchingUserIds);
        return await Task.FromResult(new SessionInfoResponse {
            Status = Enum.Parse<SessionStatus>(result.Status),
            FeedbackRating = result.Session.FeedbackRating,
            Users = result.MatchUserIds,
            CurrentQuestion = result.CurrentQuestion.ToResponse()
        });
    }
}