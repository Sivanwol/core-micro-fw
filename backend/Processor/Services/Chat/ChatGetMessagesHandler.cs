using Application.Exceptions;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Requests.Processor.Services.Chat;
using Domain.Responses.Controllers.Chat;
using MediatR;
using Serilog;
namespace Processor.Services.Chat;

public class ChatGetMessagesHandler : IRequestHandler<ChaGetMessagesRequest, MessagesResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMediator _mediator;

    public ChatGetMessagesHandler(IMediator mediator,
        IAppUserRepository appUserRepository,
        IChatRepository chatRepository) {
        _appUserRepository = appUserRepository;
        _chatRepository = chatRepository;
        _mediator = mediator;
    }


    public async Task<MessagesResponse> Handle(ChaGetMessagesRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"ChatGetMessagesHandler: [{request.SessionId}]");
        var user = await _appUserRepository.GetById(request.UserId);
        if (user == null) {
            throw new EntityNotFoundException("User", request.UserId.ToString());
        }
        var sessionExist = await _appUserRepository.HasUserSessionExist(request.SessionId, request.UserId);
        if (!sessionExist) {
            throw new EntityNotFoundException("Session", $"[{request.SessionId}-{request.UserId}]");
        }
        var result = await _chatRepository.GetMessages(request.SessionId, request.UserId, request.Limit, request.LastMessageId);
        return await Task.FromResult(result);
    }
}