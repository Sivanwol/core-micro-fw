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
using Infrastructure.GQL.Common;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;
namespace Processor.Services.User;

public class GetUsersHandler : IRequestHandler<GetUsersRequest, EntityPage<Infrastructure.GQL.User>> {
    private readonly IActivityLogRepository _activityRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly BackendApplicationConfig _config;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public GetUsersHandler(
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
    public async Task<EntityPage<Infrastructure.GQL.User>> Handle(GetUsersRequest request, CancellationToken cancellationToken) {
        await ValidateRequest(request);
        await _activityRepository.AddActivity(request.LoggedInUserId, request.LoggedInUserId, "", nameof(ApplicationUser), ActivityLogOperationType.ListOfUsers,
            "Request to get list of users", $"request user list by this filters {JsonConvert.SerializeObject(request)}", ActivityStatus.Success, request.IpAddress,
            request.UserAgent);
        var cacheKey = Cache.GetKey($"Users:{StringExtensions.CreateMD5Hash(JsonConvert.SerializeObject(request))}");
        var cacheExist = await _cacheService.IsExistAsync(cacheKey);
        if (cacheExist) {
            var cacheValue = await _cacheService.GetAsync<EntityPage<Infrastructure.GQL.User>>(cacheKey);
            if (cacheValue != null) {
                return cacheValue;
            }
        }

        var pageFilter = await BuildPageFilter(request);
        var entities = await _applicationUserRepository.GetUsers(pageFilter);
        // we enrich the entities (that are user records with roles if it has any
        var users = new List<Infrastructure.GQL.User>();

        foreach (var entity in entities.ToList()) {
            var roles = await _userManager.GetRolesAsync(entity);
            users.Add(entity.ToGql(roles));
        }

        var totalPages = await _applicationUserRepository.GetUsersTotalPages(pageFilter);
        var totalRecords = await _applicationUserRepository.GetUsersTotalRecords(pageFilter);
        var records = new EntityPage<Infrastructure.GQL.User>() {
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            HasPreviousPage = (totalPages > 1 && pageFilter.Page > 1),
            HasNextPage = (totalPages > pageFilter.Page),
            Records = users
        };
        await _cacheService.RegisterAsync(cacheKey, records);
        return records;
    }

    private async Task ValidateRequest(GetUsersRequest request) {
        if (Guid.Empty == request.LoggedInUserId) {
            Log.Logger.Error($"GetUsersHandler: Logged in User Id is Invalid");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedUser = await _applicationUserRepository.GetById(request.LoggedInUserId);
        if (loggedUser == null) {
            Log.Logger.Error($"GetUsersHandler: Logged user not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedRoles = await _userManager.GetRolesAsync(loggedUser);
        var matchedRoles = new List<string>() {
            Roles.Admin
        };
        if (!matchedRoles.Any(x => loggedRoles.Contains(x))) {
            Log.Logger.Error($"GetUsersHandler: User not authorized");
            throw new AuthorizationException();
        }
    }

    private async Task<RecordFilterPagination<ApplicationUserFilters>> BuildPageFilter(GetUsersRequest request) {
        var view = await _configurableUserViewRepository.GetViewDefinition(request.LoggedInUserId.ToString(), request.PageControl.ViewClientId);

        var pageFilter = new RecordFilterPagination<ApplicationUserFilters>() {
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
                        pageFilter.Filters.IdFilters.Add(new UserIdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "FirstName":
                        pageFilter.Filters.FirstNameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "LastName":
                        pageFilter.Filters.LastNameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "Address":
                        pageFilter.Filters.Addressilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "Email":
                        pageFilter.Filters.EmailFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "RegisterCompletedAt":
                        pageFilter.Filters.RegisterCompletedAtFilters.Add(new DateFilterField(filter.FieldName, Enums.MapEnum<FilterDateOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values?.Select(x => Convert.ToDateTime(x)).ToList()
                        });
                        break;
                }
            } else {
                switch (filter.FieldName) {
                    case "Id":
                        pageFilter.Filters.IdFilters.Add(new UserIdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "FirstName":
                        pageFilter.Filters.FirstNameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "LastName":
                        pageFilter.Filters.LastNameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Address":
                        pageFilter.Filters.Addressilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "Email":
                        pageFilter.Filters.EmailFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "RegisterCompletedAt":
                        pageFilter.Filters.RegisterCompletedAtFilters.Add(new DateFilterField(filter.FieldName, Enums.MapEnum<FilterDateOperation>(filter.FilterOperation)) {
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