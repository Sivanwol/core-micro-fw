using Application.Exceptions;
using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.Chat;
using MediatR;
using Serilog;
namespace Processor.Services.Chat;

public class ChatDeleteMessageHandler : IRequestHandler<ChatDeleteMessageRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMediator _mediator;

    public ChatDeleteMessageHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        IChatRepository chatRepository) {
        _appUserRepository = appUserRepository;
        _chatRepository = chatRepository;
        _mediator = mediator;
    }


    public async Task<EmptyResponse> Handle(ChatDeleteMessageRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"ChatDeleteMessageHandler: [{request.SessionId}-{request.UserId}-{request.MessageId}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var sessionExist = await _appUserRepository.HasUserSessionExist(request.SessionId, request.UserId);
        if (!sessionExist) {
            throw new EntityNotFoundException("Session", $"[{request.SessionId}-{request.UserId}]");
        }
        var messageExist = await _chatRepository.HasMessage(request.SessionId, request.UserId, request.MessageId);
        if (!messageExist) {
            throw new EntityNotFoundException("UserMessages", $"[{request.SessionId}-{request.UserId}-{request.MessageId}]");
        }
        await _chatRepository.DeleteMessage(request.SessionId, request.UserId, request.MessageId);
        return await Task.FromResult(new EmptyResponse());
    }
}