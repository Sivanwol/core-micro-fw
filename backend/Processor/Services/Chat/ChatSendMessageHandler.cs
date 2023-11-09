using Application.Exceptions;
using Application.Responses.Base;
using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Chat;
using MediatR;
using Serilog;
namespace Processor.Services.Chat;

public class ChatSendMessageHandler : IRequestHandler<ChatSendMessageRequest, EmptyResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMediator _mediator;

    public ChatSendMessageHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        IChatRepository chatRepository) {
        _appUserRepository = appUserRepository;
        _chatRepository = chatRepository;
        _mediator = mediator;
    }


    public async Task<EmptyResponse> Handle(ChatSendMessageRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"ChatSendMessageHandler: [{request.SessionId}-{request.UserId}-{request.ReplyId}-{request.Type}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var sessionExist = await _appUserRepository.HasUserSessionExist(request.SessionId, request.UserId);
        if (!sessionExist) {
            throw new EntityNotFoundException("Session", $"[{request.SessionId}-{request.UserId}]");
        }
        if (request.ReplyId.HasValue) {
            var messageExist = await _chatRepository.HasMessage(request.SessionId, request.UserId, request.ReplyId.Value);
            if (!messageExist) {
                throw new EntityNotFoundException("UserMessages", $"[{request.SessionId}-{request.UserId}-{request.ReplyId.Value}]");
            }
        }
        switch (request.Type) {
            case MessageType.BotMessage:
                await _chatRepository.SendMessage(request.SessionId, request.UserId, request.Message, MessageType.BotMessage);
                break;
            case MessageType.Message:
                await _chatRepository.SendMessage(request.SessionId, request.UserId, request.Message);
                break;
            case MessageType.Reply:
                if (request.ReplyId.HasValue)
                    await _chatRepository.ReplyMessage(request.SessionId, request.UserId, request.ReplyId.Value, request.Message);
                break;
        }
        return await Task.FromResult(new EmptyResponse());
    }
}