// <copyright file="MemoryCacheManager.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.Extensions.Caching.Memory;
using Platinum.Core.Abstractions.State;

namespace Platinum.Infrastructure.Services
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheManager"/> class.
        /// </summary>
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T Set<T>(string key, T obj, double minutes = 10)
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(minutes),
                AbsoluteExpiration = DateTime.Now.AddMinutes(minutes),
                Priority = CacheItemPriority.Normal,
            };

            cacheExpirationOptions.RegisterPostEvictionCallback(EvictionCallback);

            return memoryCache.Set(key, obj, cacheExpirationOptions);
        }

        public T Set<T>(string key, T obj)
        {
            return Set(key, obj, 10);
        }

        private static void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            // var message = $"Entry was evicted. Reason: {reason}.";
            // ((HomeController)state)._cache.Set(CacheKeys.CallbackMessage, message);
        }

        public T Get<T>(string key)
        {
            return memoryCache.Get<T>(key);
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
