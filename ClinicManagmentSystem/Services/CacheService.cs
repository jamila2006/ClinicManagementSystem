using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Services.Implementations
{
    public class CacheService :ICacheService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedValue)) 
            {
                Console.WriteLine($"[CACHE HIT] {cacheKey}");
                return cachedValue;
            }
            Console.WriteLine($"[CACHE MISS] {cacheKey} — DB-yə gedirik");
            var freshValue = await factory();
            _cache.Set(cacheKey, freshValue, expiration ?? DefaultExpiration);
            return freshValue;
        }

        
    }
}
