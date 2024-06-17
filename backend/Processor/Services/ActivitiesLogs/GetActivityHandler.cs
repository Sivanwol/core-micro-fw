using Application.Configs;
using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Filters.Fields;
using Domain.Persistence.Repositories.Interfaces;
using EasyCaching.Core;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.GQL.Common;
using Infrastructure.Requests.Processor.Services.ActivitiesLogs;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;
namespace Processor.Services.ActivitiesLogs;

public class GetActivityHandler : IRequestHandler<GetActivityRequest, EntityPage<Activities>> {
    private readonly IActivityLogRepository _activityRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly BackendApplicationConfig _config;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public GetActivityHandler(
        IApplicationUserRepository userRepository,
        IConfigurableUserViewRepository configurableUserViewRepository,
        ICacheService cacheService,
        IActivityLogRepository activityRepository,
        IEasyCachingProviderFactory factory,
        BackendApplicationConfig config,
        UserManager<ApplicationUser> userManager) {
        _activityRepository = activityRepository;
        _applicationUserRepository = userRepository;
        _userManager = userManager;
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
        _config = config;
    }
    public async Task<EntityPage<Activities>> Handle(GetActivityRequest request, CancellationToken cancellationToken) {
        await ValidateRequest(request);
        var cacheKey = Cache.GetKey($"Activities:{StringExtensions.CreateMD5Hash(JsonConvert.SerializeObject(request))}");
        var cacheExist = await _cacheService.IsExistAsync(cacheKey);
        if (cacheExist) {
            var cacheValue = await _cacheService.GetAsync<EntityPage<Activities>>(cacheKey);
            if (cacheValue != null) {
                return cacheValue;
            }
        }

        var pageFilter = await BuildPageFilter(request);
        var activities = await _activityRepository.GetActivities(pageFilter);
        var totalPages = await _activityRepository.GetActivitiesTotalPages(pageFilter);
        var totalRecords = await _activityRepository.GetActivitiesCount(pageFilter);
        var records = new EntityPage<Activities>() {
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            HasPreviousPage = (totalPages > 1 && pageFilter.Page > 1),
            HasNextPage = (totalPages > pageFilter.Page),
            Records = activities.Select(x => x.ToGql())
        };
        await _cacheService.RegisterAsync(cacheKey, records);
        return records;
    }

    private async Task ValidateRequest(GetActivityRequest request) {
        if (Guid.Empty == request.LoggedInUserId) {
            Log.Logger.Error($"GetUserProfileHandler: Logged in User Id is Invalid");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
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

    private async Task<RecordFilterPagination<ActivityLogFilters>> BuildPageFilter(GetActivityRequest request) {
        var view = await _configurableUserViewRepository.GetViewDefinition(request.LoggedInUserId.ToString(), request.PageControl.ViewClientId);
        var pageFilter = new RecordFilterPagination<ActivityLogFilters>() {
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
                    case "IpAddress":
                        pageFilter.Filters.IpAddressFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "UserAgent":
                        pageFilter.Filters.UserAgentFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "EntityId":
                        pageFilter.Filters.EntityIdFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "OperationType":
                        pageFilter.Filters.OperationTypeFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "Activity":
                        pageFilter.Filters.ActivityFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "Status":
                        pageFilter.Filters.StatusFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "Id":
                        pageFilter.Filters.IdFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values?.Select(x => Convert.ToInt32(x)).ToList()
                        });
                        break;

                    case "CreatedAt":
                        pageFilter.Filters.CreatedAtFilters.Add(new DateFilterField(filter.FieldName, Enums.MapEnum<FilterDateOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values?.Select(x => Convert.ToDateTime(x)).ToList()
                        });
                        break;
                }
            } else {
                switch (filter.FieldName) {
                    case "IpAddress":
                        pageFilter.Filters.IpAddressFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "UserAgent":
                        pageFilter.Filters.UserAgentFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "EntityId":
                        pageFilter.Filters.EntityIdFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "OperationType":
                        pageFilter.Filters.OperationTypeFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Activity":
                        pageFilter.Filters.ActivityFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Status":
                        pageFilter.Filters.StatusFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Id":
                        pageFilter.Filters.IdFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = Convert.ToInt32(filter.Value)
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