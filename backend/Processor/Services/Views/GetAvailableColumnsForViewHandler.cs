using Application.Constraints;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.Views;
using Infrastructure.Services.Cache;
using MediatR;
using Serilog;
namespace Processor.Services.Views;

public class GetAvailableColumnsForViewHandler : IRequestHandler<GetAvailableColumnsForViewRequest, ICollection<ViewColumn>> {
    private readonly ICacheService _cacheService;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;

    public GetAvailableColumnsForViewHandler(IConfigurableUserViewRepository configurableUserViewRepository, ICacheService cacheService) {
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
    }
    public async Task<ICollection<ViewColumn>> Handle(GetAvailableColumnsForViewRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information("Fetching all available columns for view");
        var cacheKey = Cache.GetKey($"View:Columns:{request.EntityType}");
        var hasCache = await _cacheService.IsExistAsync(cacheKey);
        if (hasCache) {
            Log.Logger.Information("Fetching all views from cache");
            var result = await _cacheService.GetAsync<ICollection<ViewColumn>>(cacheKey);
            if (result != null) {
                return result;
            }
            Log.Logger.Information("Fetching all views from cache failed");
            await _cacheService.RemoveAsync(cacheKey);
        }
        var columns = await _configurableUserViewRepository.GetAvailableColumnsForView(request);
        await _cacheService.RegisterAsync(cacheKey, columns, Cache.EFCachingTimeInMinutes);
        return columns;
    }
}