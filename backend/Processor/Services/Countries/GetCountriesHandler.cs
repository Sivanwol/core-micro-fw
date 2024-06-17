using Application.Constraints;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.Countries;
using Infrastructure.Services.Cache;
using MediatR;
using Serilog;
namespace Processor.Services.Countries;

public class GetCountriesHandler : IRequestHandler<GetCountriesRequest, IEnumerable<Country>> {
    private readonly ICacheService _cacheService;
    private readonly ICountriesRepository _countriesRepository;
    public GetCountriesHandler(ICountriesRepository countriesRepository, ICacheService cacheService) {
        _countriesRepository = countriesRepository;
        _cacheService = cacheService;
    }
    public async Task<IEnumerable<Country>> Handle(GetCountriesRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information("GetCountriesHandler: Fetching all countries");

        var cacheKey = Cache.GetKey($"COUNTRIES");
        var hasCache = await _cacheService.IsExistAsync(cacheKey);
        if (hasCache) {
            Log.Logger.Information("GetCountriesHandler: Fetching all countries from cache");
            var result = await _cacheService.GetAsync<IEnumerable<Country>>(cacheKey);
            if (result != null) {
                return result;
            }
            Log.Logger.Information("GetCountriesHandler: Fetching all countries from cache failed");
            await _cacheService.RemoveAsync(cacheKey);
        }
        var countries = (await _countriesRepository.GetAll()).Select(x => x.ToGql()).ToList();

        await _cacheService.RegisterAsync(cacheKey, countries, Cache.EFCachingTimeInMinutes);
        return countries;
    }
}