using Application.Exceptions;
using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.Chat;
using MediatR;
using Serilog;
namespace Processor.Services.Chat;

public class ChatReportUserHandler : IRequestHandler<ChatReportUserRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMediator _mediator;

    public ChatReportUserHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        IChatRepository chatRepository) {
        _appUserRepository = appUserRepository;
        _chatRepository = chatRepository;
        _mediator = mediator;
    }


    public async Task<EmptyResponse> Handle(ChatReportUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"ChatReportUserHandler: [{request.SessionId}-{request.UserId}-{request.ReportedUserId}-{request.ReportType}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var sessionExist = await _appUserRepository.HasUserSessionExist(request.SessionId, request.UserId);
        if (!sessionExist) {
            throw new EntityNotFoundException("Session", $"[{request.SessionId}-{request.UserId}]");
        }
        await _chatRepository.ReportUser(request.SessionId, request.UserId, request.ReportedUserId, request.ReportType, request.Reason);
        return await Task.FromResult(new EmptyResponse());
    }
}