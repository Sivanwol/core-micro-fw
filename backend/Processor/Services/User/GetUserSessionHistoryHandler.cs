using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Common;
using Infrastructure.Responses.Controllers.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class GetUserSessionHistory : IRequestHandler<GetUserHistoryRequest, GetUserSessionsResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public GetUserSessionHistory(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<GetUserSessionsResponse> Handle(GetUserHistoryRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetUserSessionHistory: {request.UserId}");
        var picks = await _appUserRepository.GetSessionHistory(request.UserId);
        var response = new GetUserSessionsResponse();
        response.Sessions = new List<MatchingUser>();
        var sessions = picks.Select(userPicks => new MatchingUser {
                UserId = userPicks.Match.MatchedUserId,
                FirstName = userPicks.MatchUser.FirstName,
                LastName = userPicks.MatchUser.LastName,
                MainMediaUrl = userPicks.MatchMedia.FileUrl,
                MainMediaWidth = userPicks.MatchMedia.Width,
                MainMediaHeight = userPicks.MatchMedia.Height
            })
            .ToList();
        response.Sessions = sessions;
        return await Task.FromResult(response);
    }
}