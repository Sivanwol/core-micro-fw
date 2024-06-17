using Application.Constraints;
using Application.Exceptions;
using Application.Responses.Base;
using Application.Utils;
using Domain.DTO.Client;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Client;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
namespace Processor.Services.Client;

public class UpdateClientHandler : IRequestHandler<UpdateClientRequest,EmptyResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly IClientRepository _clientRepository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateClientHandler(
        IApplicationUserRepository userRepository,
        ICacheService cacheService,
        IActivityLogRepository activityRepository,
        ICountriesRepository countriesRepository,
        IClientRepository clientRepository,
        UserManager<ApplicationUser> userManager) {
        _applicationUserRepository = userRepository;
        _activityLogRepository = activityRepository;
        _userManager = userManager;
        _clientRepository = clientRepository;
        _cacheService = cacheService;
        _countriesRepository = countriesRepository;
    }
    public async Task<EmptyResponse> Handle(UpdateClientRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"UpdateClientHandler: been trigger");
        await ValidateRequest(request);
        Log.Logger.Information($"Clear Cache");
        var cacheKey = Cache.GetKey("Clients:List");
        await _cacheService.RemovePatternAsync(cacheKey);
        Log.Logger.Information($"Register new client");
        var client = new CreateClient {
            OwnerUserId = request.LoggedInUserId.ToString(),
            Name = request.Name,
            Description = request.Description,
            Website = request.Website,
            CountryId = request.CountryId,
            Address = request.Address,
            City = request.City
        };
        if (request.ParentId.HasValue) {
            await _clientRepository.Update(request.ClientId, request.ParentId.Value, client);
        } else {
            await _clientRepository.Update(request.ClientId, client);
        }
        await _activityLogRepository.AddActivity(
            request.LoggedInUserId,
            request.ClientId.ToString(),
            nameof(Clients),
            ActivityLogOperationType.ClientUpdate,
            $"Client {client.Name} been update", $"Client {client.Name} been ppdate by {request.LoggedInUserId} with id {request.ClientId}",
            ActivityStatus.Success,
            request.IpAddress,
            request.UserAgent);
        return new EmptyResponse();
    }

    private async Task ValidateRequest(UpdateClientRequest request) {
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

        var country = await _countriesRepository.GetById(request.CountryId);
        if (country?.SupportedAt == null) {
            Log.Logger.Error($"Country not found");
            throw new EntityNotFoundException(nameof(Countries), request.CountryId.ToString());
        }
        var client = await _clientRepository.GetById(request.ClientId);
        if (client == null) {
            Log.Logger.Error($"Client not found");
            throw new EntityNotFoundException(nameof(Clients), request.ClientId.ToString());
        }

        if (request.ParentId.HasValue) {
            var parent = await _clientRepository.GetById(request.ParentId.Value);
            if (parent == null) {
                Log.Logger.Error($"Parent not found");
                throw new EntityNotFoundException(nameof(Clients), request.ParentId.Value.ToString());
            }
        }
    }
}