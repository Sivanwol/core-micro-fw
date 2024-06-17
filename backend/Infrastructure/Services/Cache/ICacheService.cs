namespace Infrastructure.Services.Cache;

public interface ICacheService {
    Task<T?> GetAsync<T>(string key);
    Task RegisterAsync<T>(string key, T value, int? minutes = null);
    Task RemoveAsync(string key);
    Task RemovePatternAsync(string pattern);
    Task<bool> IsExistAsync(string key);
    Task RegisterListItemAsync<T>(string cacheKey, string fieldId, T records, int? minutes = null);
    Task<T?> GetListItemAsync<T>(string cacheKey, string fieldId);
    Task<IEnumerable<T>?> GetListItemsAsync<T>(string cacheKey);
    Task<bool> IsListItemExistAsync(string cacheKey, string fieldId);
    Task RemoveListItemAsync(string cacheKey, IList<string> fieldIds);
}