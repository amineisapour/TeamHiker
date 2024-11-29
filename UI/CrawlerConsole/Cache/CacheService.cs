using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Cache
{
    public class CacheService
    {
        private static readonly Lazy<CacheService> _instance = new Lazy<CacheService>(() => new CacheService());
        private readonly MemoryCache _cache;

        private CacheService()
        {
            _cache = new MemoryCache("TeamHikerCacheService");
        }

        public static CacheService Instance => _instance.Value;

        public T? Get<T>(string key)
        {
            if (_cache.Contains(key))
            {
                return (T)_cache.Get(key);
            }
            return default(T);
        }

        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration };
            _cache.Set(key, value, policy);
        }

        public void Remove(string key)
        {
            if (_cache.Contains(key))
            {
                _cache.Remove(key);
            }
        }

        public void RemoveAll()
        {
            var cacheItems = new List<string>();
            foreach (var item in _cache)
            {
                cacheItems.Add(item.Key);
            }

            foreach (var key in cacheItems)
            {
                _cache.Remove(key);
            }
        }
    }
}
