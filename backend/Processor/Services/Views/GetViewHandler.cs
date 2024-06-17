using Application.Constraints;
using Application.Exceptions;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.Views;
using Infrastructure.Services.Cache;
using MediatR;
using Serilog;
namespace Processor.Services.Views;

public class GetViewHandler : IRequestHandler<GetViewRequest, View> {
    private readonly ICacheService _cacheService;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;

    public GetViewHandler(IConfigurableUserViewRepository configurableUserViewRepository, ICacheService cacheService) {
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
    }
    public async Task<View> Handle(GetViewRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information("Fetching all views");
        var cacheKey = Cache.GetKey($"View:{request.UserId}");
        var hasCache = await _cacheService.IsListItemExistAsync(cacheKey, request.ViewClientKey.ToString());
        if (hasCache) {
            Log.Logger.Information("Fetching all views from cache");
            var result = await _cacheService.GetListItemAsync<View>(cacheKey, request.ViewClientKey.ToString());
            if (result != null) {
                return result;
            }
            Log.Logger.Information("Fetching all views from cache failed");
            await _cacheService.RemoveListItemAsync(cacheKey, new List<string>() {
                request.ViewClientKey.ToString()
            });
        }
        var view = await _configurableUserViewRepository.GetView(request.UserId, request.ViewClientKey);
        if (view == null) {
            throw new EntityNotFoundException("ConfigurableUserView", request.ViewClientKey.ToString());
        }

        await _cacheService.RegisterListItemAsync<View>(cacheKey, request.ViewClientKey.ToString(), view, Cache.EFCachingTimeInMinutes);
        return view;
    }
}