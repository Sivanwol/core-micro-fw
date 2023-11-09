using Application.Exceptions;
using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.Session;
using MediatR;
using Serilog;
namespace Processor.Services.Session;

public class SessionReplyToQuestionHandler : IRequestHandler<SessionReplyToQuestionRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;
    private readonly ISessionRepository _sessionRepository;

    public SessionReplyToQuestionHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        ISessionRepository sessionRepository) {
        _appUserRepository = appUserRepository;
        _sessionRepository = sessionRepository;
        _mediator = mediator;
    }


    public async Task<EmptyResponse> Handle(SessionReplyToQuestionRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"SessionReplyToQuestionHandler: [{request.SessionId}-{request.UserId}-{request.QuestionId}]");
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
        await _sessionRepository.ReplyToQuestion(request.SessionId, request.UserId, request.QuestionId, request.Message!);
        return await Task.FromResult(new EmptyResponse());
    }
}