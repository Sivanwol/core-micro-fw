using Application.Constraints;
using Application.Exceptions;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.Views;
using Infrastructure.Services.Cache;
using MediatR;
using Serilog;
namespace Processor.Services.Views;

public class UpdateViewHandler : IRequestHandler<UpdateViewRequest, View> {
    private readonly ICacheService _cacheService;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;

    public UpdateViewHandler(IConfigurableUserViewRepository configurableUserViewRepository, ICacheService cacheService) {
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
    }
    public async Task<View> Handle(UpdateViewRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information("Fetching all views");
        var basedView = await _configurableUserViewRepository.GetView(request.UserId, request.ViewClientKey);
        if (basedView == null) {
            throw new EntityNotFoundException("ConfigurableUserView", request.ViewClientKey.ToString());
        }
        var cacheKey = Cache.GetKey($"Views:{request.UserId}");
        var hasCache = await _cacheService.IsExistAsync(cacheKey);
        if (hasCache) {
            await _cacheService.RemoveAsync(cacheKey);
        }
        cacheKey = Cache.GetKey($"View:{request.UserId}");
        hasCache = await _cacheService.IsListItemExistAsync(cacheKey, request.ViewClientKey.ToString());
        if (hasCache) {
            await _cacheService.RemoveListItemAsync(cacheKey, new List<string>() {
                request.ViewClientKey.ToString()
            });
        }
        var view = await _configurableUserViewRepository.UpdateView(request);
        return view;
    }
}