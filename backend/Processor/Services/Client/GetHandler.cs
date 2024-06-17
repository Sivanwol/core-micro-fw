using Application.Configs;
using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Filters.Fields;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL.Common;
using Infrastructure.Requests.Processor.Services.Client;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;
namespace Processor.Services.Client;

public class GetHandler : IRequestHandler<GetClientRequest, Infrastructure.GQL.Client?> {
    private readonly IActivityLogRepository _activityRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly IClientRepository _clientRepository;
    private readonly BackendApplicationConfig _config;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public GetHandler(
        IApplicationUserRepository userRepository,
        IConfigurableUserViewRepository configurableUserViewRepository,
        ICacheService cacheService,
        IActivityLogRepository activityRepository,
        IClientRepository clientRepository,
        BackendApplicationConfig config,
        UserManager<ApplicationUser> userManager) {
        _activityRepository = activityRepository;
        _applicationUserRepository = userRepository;
        _userManager = userManager;
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
        _config = config;
        _clientRepository = clientRepository;
    }
    public async Task<Infrastructure.GQL.Client?> Handle(GetClientRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"GetAllHandler: been trigger");
        await ValidateRequest(request);
        var cacheKey = Cache.GetKey($"Client:{request.ClientId}");
        var cacheExist = await _cacheService.IsExistAsync(cacheKey);
        if (cacheExist) {
            var cacheValue = await _cacheService.GetAsync<Infrastructure.GQL.Client>(cacheKey);
            if (cacheValue != null) {
                return cacheValue;
            }
        }

        var results = await _clientRepository.GetById(request.ClientId);
        await _cacheService.RegisterAsync(cacheKey, results);
        await _activityRepository.AddActivity(
            request.LoggedInUserId,
            nameof(Clients),
            ActivityLogOperationType.ClientList,
            $"Client {request.ClientId} been fetched",
            $"Client {request.ClientId} been fetched by {request.LoggedInUserId}",
            ActivityStatus.Success,
            request.IpAddress,
            request.UserAgent);
        
        return results?.ToGql();
    }

    private async Task ValidateRequest(GetClientRequest request) {
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
            Roles.Admin,
            Roles.IT
        };
        if (!matchedRoles.Any(x => loggedRoles.Contains(x))) {
            Log.Logger.Error($"User not authorized");
            throw new AuthorizationException();
        }
    }

    private async Task<RecordFilterPagination<ClientFilters>> BuildPageFilter(GetClientsRequest request) {
        var view = await _configurableUserViewRepository.GetViewDefinition(request.LoggedInUserId.ToString(), request.PageControl.ViewClientId);
        var pageFilter = new RecordFilterPagination<ClientFilters>() {
            Page = request.PageControl.Page,
            PageSize = (int)request.PageControl.PageSize,
            SortByField = request.PageControl.SortByField!,
            SortDirection = request.PageControl.SortDirection,
            FilterCollectionOperation = request.PageControl.FilterCollectionOperation,
            SelectedColumns = view.Columns ?? throw new InvalidOperationException()
        };

        foreach (var filter in request.PageControl.Filters) {
            if (IsMultiValueFilter(filter.FilterOperation)) {
                switch (filter.FieldName) {
                    case "Id":
                        pageFilter.Filters.IdFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToInt32(x))
                        });
                        break;
                    case "OwnerUserId":
                        pageFilter.Filters.OwnerUserFilters.Add(new UserIdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToString(x))
                        });
                        break;
                    case "Name":
                        pageFilter.Filters.NameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToString(x))
                        });
                        break;
                    case "CountryId":
                        pageFilter.Filters.CountryFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToInt32(x))
                        });
                        break;
                    case "City":
                        pageFilter.Filters.CityFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToString(x))
                        });
                        break;
                    case "Address":
                        pageFilter.Filters.AddressFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToString(x))
                        });
                        break;
                    case "CreatedAt":
                        pageFilter.Filters.CreatedAtFilters.Add(new DateFilterField(filter.FieldName, Enums.MapEnum<FilterDateOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values!.ToList().ConvertAll(x => Convert.ToDateTime(x))
                        });
                        break;
                }
            } else {
                switch (filter.FieldName) {
                    case "Id":
                        pageFilter.Filters.IdFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = Convert.ToInt32(filter.Value)
                        });
                        break;
                    case "OwnerUserId":
                        pageFilter.Filters.OwnerUserFilters.Add(new UserIdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Name":
                        pageFilter.Filters.NameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "CountryId":
                        pageFilter.Filters.CountryFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = Convert.ToInt32(filter.Value)
                        });
                        break;
                    case "City":
                        pageFilter.Filters.CityFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Address":
                        pageFilter.Filters.AddressFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "CreatedAt":
                        pageFilter.Filters.CreatedAtFilters.Add(new DateFilterField(filter.FieldName, Enums.MapEnum<FilterDateOperation>(filter.FilterOperation)) {
                            FilterValue = Convert.ToDateTime(filter.Value)
                        });
                        break;
                }
            }
        }
        return pageFilter;
    }

    private bool IsMultiValueFilter(FilterOperations filterOperation) {
        return filterOperation is FilterOperations.In or FilterOperations.NotIn or FilterOperations.Between or FilterOperations.NotBetween;
    }
}