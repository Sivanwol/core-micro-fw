using Domain.Persistence.Interfaces.Repositories;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Common;
using Infrastructure.Responses.Controllers.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class GetUserPicksHandler : IRequestHandler<GetUserPicksRequest, GetUserPickResponse> {
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMediator _mediator;

    public GetUserPicksHandler(IMediator mediator, IAppUserRepository appUserRepository) {
        _appUserRepository = appUserRepository;
        _mediator = mediator;
    }

    public async Task<GetUserPickResponse> Handle(GetUserPicksRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetUserPicksHandler: {request.UserId}");
        var picks = await _appUserRepository.GetRecommendPicks(request.UserId);
        var response = new GetUserPickResponse();
        var newMatchs = new List<MatchingUser>();
        var openMatchs = new List<MatchingUser>();
        response.NewMatchs = new List<MatchingUser>();
        response.OpenMatchs = new List<MatchingUser>();
        foreach (var userPicks in picks) {
            var matchingUser = new MatchingUser {
                UserId = userPicks.Match.MatchedUserId,
                FirstName = userPicks.MatchUser.FirstName,
                LastName = userPicks.MatchUser.LastName,
                MainMediaUrl = userPicks.MatchMedia.FileUrl,
                MainMediaWidth = userPicks.MatchMedia.Width,
                MainMediaHeight = userPicks.MatchMedia.Height
            };
            if (userPicks.Match.Status == UserMatchingStatus.New) {
                newMatchs.Add(matchingUser);
            } else {
                openMatchs.Add(matchingUser);
            }
        }
        response.NewMatchs = newMatchs;
        response.OpenMatchs = openMatchs;
        return await Task.FromResult(response);
    }
}