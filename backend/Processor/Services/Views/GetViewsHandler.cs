using Application.Constraints;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.Views;
using Infrastructure.Services.Cache;
using MediatR;
using Serilog;
namespace Processor.Services.Views;

public class GetViewsHandler : IRequestHandler<GetViewsRequest, IEnumerable<View>> {
    private readonly ICacheService _cacheService;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;

    public GetViewsHandler(IConfigurableUserViewRepository configurableUserViewRepository, ICacheService cacheService) {
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
    }
    public async Task<IEnumerable<View>> Handle(GetViewsRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information("Fetching all views");
        var cacheKey = Cache.GetKey($"Views:{request.UserId}");
        var hasCache = await _cacheService.IsExistAsync(cacheKey);
        if (hasCache) {
            Log.Logger.Information("Fetching all views from cache");
            var result = await _cacheService.GetAsync<IEnumerable<View>>(cacheKey);
            if (result != null) {
                return result;
            }
            Log.Logger.Information("Fetching all views from cache failed");
            await _cacheService.RemoveAsync(cacheKey);
        }
        var views = await _configurableUserViewRepository.GetViews(request.UserId);
        await _cacheService.RegisterAsync(cacheKey, views, Cache.EFCachingTimeInMinutes);
        return views;
    }
}