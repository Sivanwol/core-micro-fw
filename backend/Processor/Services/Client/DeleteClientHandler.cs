using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Client;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
namespace Processor.Services.Client;

public class DeleteClientHandler : IRequestHandler<DeleteClientRequest> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly IClientRepository _clientRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public DeleteClientHandler(
        IApplicationUserRepository userRepository,
        ICacheService cacheService,
        IActivityLogRepository activityRepository,
        IClientRepository clientRepository,
        UserManager<ApplicationUser> userManager) {
        _applicationUserRepository = userRepository;
        _activityLogRepository = activityRepository;
        _userManager = userManager;
        _clientRepository = clientRepository;
        _cacheService = cacheService;
    }
    public async Task Handle(DeleteClientRequest request, CancellationToken cancellationToken) {
        await ValidateRequest(request);
        Log.Logger.Information($"DeleteClientHandler: been trigger");
        var cacheKey = Cache.GetKey("Clients:List");
        await _cacheService.RemovePatternAsync(cacheKey);
        Log.Logger.Information($"Register new client");
        await _clientRepository.Delete(request.ClientId);
        await _activityLogRepository.AddActivity(
            request.LoggedInUserId,
            request.ClientId.ToString(),
            nameof(Clients),
            ActivityLogOperationType.ClientDelete,
            $"Client {request.ClientId} deleted",
            $"Client {request.ClientId} deleted",
            ActivityStatus.Success,
            request.IpAddress,
            request.UserAgent);
    }

    private async Task ValidateRequest(DeleteClientRequest request) {
        if (Guid.Empty == request.LoggedInUserId) {
            Log.Logger.Error($"Logged in User Id is Invalid");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedUser = await _applicationUserRepository.GetById(request.LoggedInUserId);
        if (loggedUser == null) {
            Log.Logger.Error($"Logged user not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedRoles = await _userManager.GetRolesAsync(loggedUser);
        var matchedRoles = new List<string>() {
            Roles.Admin
        };
        if (!matchedRoles.Any(x => loggedRoles.Contains(x))) {
            Log.Logger.Error($"User not authorized");
            throw new AuthorizationException();
        }
        var client = await _clientRepository.GetById(request.ClientId);
        if (client == null) {
            Log.Logger.Error($"Client not found");
            throw new EntityNotFoundException(nameof(Clients), request.ClientId.ToString());
        }
    }
}