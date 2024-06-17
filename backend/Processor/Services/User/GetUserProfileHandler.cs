using Application.Constraints;
using Application.Exceptions;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
namespace Processor.Services.User;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileRequest, Infrastructure.GQL.User> {
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILanguagesRepository _languagesRepository;
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserProfileHandler(IMediator mediator,
        IApplicationUserRepository applicationUserRepository,
        UserManager<ApplicationUser> userManager,
        ICountriesRepository _countriesRepository,
        ILanguagesRepository _languagesRepository) {
        _applicationUserRepository = applicationUserRepository;
        _mediator = mediator;
        _userManager = userManager;
        this._countriesRepository = _countriesRepository;
        this._languagesRepository = _languagesRepository;
    }

    public async Task<Infrastructure.GQL.User> Handle(GetUserProfileRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetUserProfileHandler: {request.UserId}");
        if (request.UserId != request.LoggedInUserId) {
            var loggedUser = await _applicationUserRepository.GetById(request.LoggedInUserId);
            if (loggedUser == null) {
                Log.Logger.Error($"GetUserProfileHandler: Logged user not found");
                throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
            }
            var loggedRoles = await _userManager.GetRolesAsync(loggedUser);
            var matchedRoles = new List<string>() {
                Roles.Admin,
                Roles.IT
            };
            if (!matchedRoles.Any(x => loggedRoles.Contains(x))) {
                Log.Logger.Error($"GetUserProfileHandler: User not authorized");
                throw new AuthorizationException();
            }
        }
        var user = await _applicationUserRepository.GetById(request.UserId);
        if (user == null) {
            Log.Logger.Error($"GetUserProfileHandler: User not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.UserId.ToString());
        }
        var roles = await _userManager.GetRolesAsync(user);
        return user.ToGql(roles);
    }
}