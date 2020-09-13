using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

namespace TradingPortal.Core.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private CancellationTokenSource _resetCacheToken;
        protected static readonly HashSet<string> CacheKeys = new HashSet<string>();

        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _resetCacheToken = new CancellationTokenSource();
        }

        public bool IsSet(string key)
        {
            string isSet;
            _memoryCache.TryGetValue(key, out isSet);
            return isSet != null;
        }

        public void Set(string key, object data, int cacheTime)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }

            _resetCacheToken = new CancellationTokenSource();
            CacheKeys.Clear();
        }

        public TCachedObject Get<TCachedObject>(string key)
        {
            return _memoryCache.TryGetValue(key, out TCachedObject value) ?
            value : default(TCachedObject);
        }

        public void RemoveByPattern(string pattern)
        {
            var keysToRemove = CacheKeys
                .Where(k => Regex.IsMatch(k, pattern, RegexOptions.IgnoreCase))
                .ToArray();
            foreach (var ktr in keysToRemove)
                Remove(ktr);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            CacheKeys.Remove(key);
        }

        public void Set<TCachedObject>(string key, TCachedObject value, uint secondsCacheTime)
        {
            var options = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.Normal)
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(secondsCacheTime));
            options.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));

            _memoryCache.Set(key, value, options);
            CacheKeys.Add(key);
        }
    }
}
