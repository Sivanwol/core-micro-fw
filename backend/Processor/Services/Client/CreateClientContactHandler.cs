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

public class CreateClientContactHandler : IRequestHandler<CreateClientContractRequest, EmptyResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly IClientRepository _clientRepository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public CreateClientContactHandler(
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
    public async Task<EmptyResponse> Handle(CreateClientContractRequest request, CancellationToken cancellationToken) {
        await ValidateRequest(request);
        Log.Logger.Information($"CreateClientContactHandler: been trigger");
        var cacheKey = Cache.GetKey("Clients:Contacts:List");
        await _cacheService.RemovePatternAsync(cacheKey);
        Log.Logger.Information($"Register new client");
        var clientContact = new CreateClientContact() {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone1 = request.Phone1,
            Phone2 = request.Phone2,
            Fax = request.Fax,
            Whatsapp = request.Whatsapp,
            CountryId = request.CountryId,
            PostalCode = request.PostalCode,
            Address = request.Address,
            City = request.City,
            State = request.State,
            JobTitle = request.JobTitle,
            Department = request.Department,
            Company = request.Company,
            Notes = request.Notes ?? "",
        };
        var clientContactId = await _clientRepository.AddContact(request.ClientId, clientContact);
        await _activityLogRepository.AddActivity(
            request.LoggedInUserId,
            clientContactId.ToString(),
            nameof(Clients),
            ActivityLogOperationType.ClientContactCreate,
            $"Client Contact {request.FirstName}, {request.LastName} been create",
            $"Client Contact on client {request.ClientId} been create by {request.LoggedInUserId} with id {clientContactId}",
            ActivityStatus.Success,
            request.IpAddress,
            request.UserAgent);

        return new EmptyResponse();
    }

    private async Task ValidateRequest(CreateClientContractRequest request) {
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
    }
}