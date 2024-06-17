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
using Infrastructure.Requests.Processor.Services.Vendor;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;

namespace Processor.Services.User;

public class GetVendorsHandler : IRequestHandler<GetVendorsRequest, EntityPage<Infrastructure.GQL.Vendor>> {
    private readonly IActivityLogRepository _activityRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICacheService _cacheService;
    private readonly BackendApplicationConfig _config;
    private readonly IVendorRepository _vendorRepository;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public GetVendorsHandler(
        IApplicationUserRepository userRepository,
        IConfigurableUserViewRepository configurableUserViewRepository,
        ICacheService cacheService,
        IActivityLogRepository activityRepository,
        IVendorRepository vendorRepository,
        IEasyCachingProviderFactory factory,
        BackendApplicationConfig config,
        UserManager<ApplicationUser> userManager) {
        _activityRepository = activityRepository;
        _applicationUserRepository = userRepository;
        _userManager = userManager;
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
        _config = config;
        _vendorRepository = vendorRepository;
    }
    public async Task<EntityPage<Infrastructure.GQL.Vendor>> Handle(GetVendorsRequest request, CancellationToken cancellationToken) {
        await ValidateRequest(request);
        var cacheKey = Cache.GetKey($"Users:{StringExtensions.CreateMD5Hash(JsonConvert.SerializeObject(request))}");
        var cacheExist = await _cacheService.IsExistAsync(cacheKey);
        if (cacheExist) {
            var cacheValue = await _cacheService.GetAsync<EntityPage<Infrastructure.GQL.Vendor>>(cacheKey);
            if (cacheValue != null) {
                return cacheValue;
            }
        }

        var pageFilter = await BuildPageFilter(request);
        var entities = await _vendorRepository.GetVendors(pageFilter);

        var totalPages = await _vendorRepository.GetVendorsTotalPages(pageFilter);
        var totalRecords = await _vendorRepository.GetVendorsTotalRecords(pageFilter);
        var records = new EntityPage<Infrastructure.GQL.Vendor>() {
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            HasPreviousPage = (totalPages > 1 && pageFilter.Page > 1),
            HasNextPage = (totalPages > pageFilter.Page),
            Records = entities.Select(x => x.ToGql()).ToList()
        };
        await _cacheService.RegisterAsync(cacheKey, records);
        return records;
    }

    private async Task ValidateRequest(GetVendorsRequest request) {
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

    private async Task<RecordFilterPagination<VendorFilters>> BuildPageFilter(GetVendorsRequest request) {
        var view = await _configurableUserViewRepository.GetViewDefinition(request.LoggedInUserId.ToString(), request.PageControl.ViewClientId);

        var pageFilter = new RecordFilterPagination<VendorFilters>() {
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
                    case "id":
                        pageFilter.Filters.IdFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values?.Select(x => Convert.ToInt32(x)).ToList()
                        });
                        break;
                    case "name":
                        pageFilter.Filters.NameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values
                        });
                        break;
                    case "country_id":
                        pageFilter.Filters.CountryFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values?.Select(x => Convert.ToInt32(x)).ToList()
                        });
                        break;
                    case "created_at":
                        pageFilter.Filters.CreatedAtFilters.Add(new DateFilterField(filter.FieldName, Enums.MapEnum<FilterDateOperation>(filter.FilterOperation)) {
                            FilterValues = filter.Values?.Select(x => Convert.ToDateTime(x)).ToList()
                        });
                        break;
                }
            } else {
                switch (filter.FieldName) {
                    case "id":
                        pageFilter.Filters.IdFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = Convert.ToInt32(filter.Value)
                        });
                        break;
                    case "name":
                        pageFilter.Filters.NameFilters.Add(new StringFilterField(filter.FieldName, Enums.MapEnum<FilterStringOperation>(filter.FilterOperation)) {
                            FilterValue = filter.Value
                        });
                        break;
                    case "country_id":
                        pageFilter.Filters.CountryFilters.Add(new IdFilterField(filter.FieldName, Enums.MapEnum<FilterIDOperation>(filter.FilterOperation)) {
                            FilterValue = Convert.ToInt32(filter.Value)
                        });
                        break;
                    case "created_at":
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