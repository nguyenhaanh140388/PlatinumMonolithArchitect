using Anhny010920.Core.Utilities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anhny010920.Core.Extensions
{
    public static class DistributedCacheExtensions
    {    /// <summary>
         /// Gets the specified distributed cache.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="distributedCache">The distributed cache.</param>
         /// <param name="key">The key.</param>
         /// <param name="obj">The object.</param>
         /// <param name="distributedCacheEntryOptions">The distributed cache entry options.</param>
         /// <returns>Result.</returns>
        public static T Set<T>(this IDistributedCache distributedCache, string key, T obj, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            distributedCache.Set(key, SerializeUtils.Serialize(obj), distributedCacheEntryOptions);
            return obj;
        }

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="distributedCacheEntryOptions">The distributed cache entry options.</param>
        /// <param name="token">The token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T obj, DistributedCacheEntryOptions distributedCacheEntryOptions, CancellationToken token = default)
        {
            await distributedCache.SetAsync(key, SerializeUtils.Serialize(obj), distributedCacheEntryOptions, token);
        }

    }
}
