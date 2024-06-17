namespace Application.Constraints;

public static class Cache {
    public static string ProviderName = "Redis";
    public static string CachePrefix = "APP_CACHE_EF";
    public static string SerializerName = "APP_CACHE";
    public static int EFCachingTimeInMinutes = 60;

    public static string GetKey(string key) {
        return $"{CachePrefix}:{key}";
    }
}