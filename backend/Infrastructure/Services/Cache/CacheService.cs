using Application.Configs;
using EasyCaching.Core;
using Newtonsoft.Json;
using Serilog;
namespace Infrastructure.Services.Cache;

public class CacheService : ICacheService {
    private readonly IRedisCachingProvider _cache;
    private readonly BackendApplicationConfig _config;
    public CacheService(IEasyCachingProviderFactory cache, BackendApplicationConfig config) {
        _cache = cache.GetRedisProvider(Application.Constraints.Cache.ProviderName);
        _config = config;
    }
    public async Task<T?> GetAsync<T>(string cacheKey) {
        if (!_config.EnableCache)
            return default(T);
        Log.Logger.Information($"FetchCache: {cacheKey}");

        if (await _cache.KeyExistsAsync(cacheKey) == false) {
            Log.Logger.Information($"FetchCache: {cacheKey} - Not Found");
            return default(T);
        }
        Log.Logger.Information($"FetchCache: {cacheKey} - Found");
        var cacheValue = await _cache.StringGetAsync(cacheKey);
        return !string.IsNullOrEmpty(cacheValue) ? JsonConvert.DeserializeObject<T>(cacheValue!)! : default(T);
    }

    public async Task RegisterListItemAsync<T>(string cacheKey, string fieldId, T records, int? minutes = null) {
        if (!_config.EnableCache) {
            return;
        }
        Log.Logger.Information("RegisterCache: {CacheKey} - {FieldId}", cacheKey, fieldId);
        if (await _cache.KeyExistsAsync(cacheKey)) {
            await RemoveAsync(cacheKey);
        }

        await _cache.HSetAsync(cacheKey, fieldId, JsonConvert.SerializeObject(records));
        await _cache.KeyExpireAsync(cacheKey, TimeSpan.FromMinutes(minutes ?? Application.Constraints.Cache.EFCachingTimeInMinutes).Seconds);
    }

    public async Task<T?> GetListItemAsync<T>(string cacheKey, string fieldId) {
        if (!_config.EnableCache)
            return default(T);
        Log.Logger.Information("GetListItemAsync: {CacheKey} - {FieldId}", cacheKey, fieldId);

        if (await _cache.KeyExistsAsync(cacheKey) == false) {
            Log.Logger.Information("GetListItemAsync: {CacheKey} - {FieldId}- Not Found", cacheKey, fieldId);
            return default(T);
        }
        Log.Logger.Information("GetListItemAsync: {CacheKey} - {FieldId}- Found", cacheKey, fieldId);
        var cacheValue = await _cache.HGetAsync(cacheKey, fieldId);
        return !string.IsNullOrEmpty(cacheValue) ? JsonConvert.DeserializeObject<T>(cacheValue!)! : default(T);
    }

    public async Task<IEnumerable<T>?> GetListItemsAsync<T>(string cacheKey) {
        if (!_config.EnableCache)
            return null;
        Log.Logger.Information("GetListItemsAsync: {CacheKey}", cacheKey);

        if (await _cache.KeyExistsAsync(cacheKey) == false) {
            Log.Logger.Information("GetListItemsAsync: {CacheKey} - Not Found", cacheKey);
            return null;
        }
        Log.Logger.Information("GetListItemsAsync: {CacheKey} - Found", cacheKey);
        var cacheValue = await _cache.HGetAllAsync(cacheKey);
        return cacheValue.Select(x => JsonConvert.DeserializeObject<T>(x.Value)!);
    }

    public async Task<bool> IsListItemExistAsync(string cacheKey, string fieldId) {
        if (!_config.EnableCache) {
            return false;
        }
        Log.Logger.Information("IsListItemExistAsync: {CacheKey}", cacheKey);
        var isExist = await _cache.HExistsAsync(cacheKey, fieldId);
        return isExist;
    }

    public async Task RemoveListItemAsync(string cacheKey, IList<string> fieldIds) {
        if (!_config.EnableCache) {
            return;
        }
        Log.Logger.Information("RemoveListItemAsync: {CacheKey}", cacheKey);
        await _cache.HDelAsync(cacheKey, fieldIds);
    }

    public async Task RegisterAsync<T>(string cacheKey, T records, int? minutes = null) {
        if (!_config.EnableCache) {
            return;
        }
        Log.Logger.Information($"RegisterCache: {cacheKey}");
        if (await _cache.KeyExistsAsync(cacheKey)) {
            await RemoveAsync(cacheKey);
        }

        await _cache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(records), TimeSpan.FromMinutes(minutes ?? Application.Constraints.Cache.EFCachingTimeInMinutes));
    }

    public async Task RemoveAsync(string cacheKey) {
        if (!_config.EnableCache) {
            return;
        }
        Log.Logger.Information($"RemoveCache: {cacheKey}");
        await _cache.KeyDelAsync($"{cacheKey}");
    }

    public async Task RemovePatternAsync(string pattern) {
        if (!_config.EnableCache) {
            return;
        }
        Log.Logger.Information($"RemovePattern: {pattern}:*");
        await _cache.KeyDelAsync($"{pattern}:*");
    }

    public async Task<bool> IsExistAsync(string cacheKey) {
        if (!_config.EnableCache) {
            return false;
        }
        Log.Logger.Information($"IsExistAsync: {cacheKey}");
        var isExist = await _cache.KeyExistsAsync(cacheKey);
        return isExist;
    }
}