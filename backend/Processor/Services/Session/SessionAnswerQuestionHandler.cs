using Application.Exceptions;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Session;
using Infrastructure.Responses.Controllers.Session;
using MediatR;
using Serilog;
namespace Processor.Services.Session;

public class SessionAnswerQuestionHandler : IRequestHandler<SessionAnswerQuestionRequest, SessionInfoResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;
    private readonly ISessionRepository _sessionRepository;

    public SessionAnswerQuestionHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        ISessionRepository sessionRepository) {
        _appUserRepository = appUserRepository;
        _sessionRepository = sessionRepository;
        _mediator = mediator;
    }


    public async Task<SessionInfoResponse> Handle(SessionAnswerQuestionRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"SessionAnswerQuestionHandler: [{request.SessionId}-{request.UserId}-{request.QuestionId}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var sessionExist = await _appUserRepository.HasUserSessionExist(request.SessionId, request.UserId);
        if (!sessionExist) {
            throw new EntityNotFoundException("Session", $"[{request.SessionId}-{request.UserId}]");
        }
        var questionExist = await _sessionRepository.HasSessionQuestionExist(request.SessionId, request.UserId, request.QuestionId);
        if (!questionExist) {
            throw new EntityNotFoundException("SessionAnswers", $"[{request.SessionId}-{request.UserId}-{request.QuestionId}]");
        }
        var result = request.AnswerId.HasValue
            ? await _sessionRepository.AnswerQuestion(request.SessionId, request.UserId, request.QuestionId, request.AnswerId.Value)
            : await _sessionRepository.AnswerQuestion(request.SessionId, request.UserId, request.QuestionId, request.AnswerText!);
        return await Task.FromResult(new SessionInfoResponse {
            Status = Enum.Parse<SessionStatus>(result.Status),
            FeedbackRating = result.Session.FeedbackRating,
            Users = result.MatchUserIds,
            CurrentQuestion = result.CurrentQuestion.ToResponse()
        });
    }
}