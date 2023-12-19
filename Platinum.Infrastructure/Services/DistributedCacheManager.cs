// <copyright file="DistributedCacheManager.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Infrastructure.Services
{
    using Microsoft.Extensions.Caching.Distributed;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// DistributedCacheManager.
    /// </summary>
    /// <seealso cref="IDistributedCacheManager" />
    public class DistributedCacheManager : IDistributedCacheManager
    {
        private IDistributedCacheRepository distributedCacheRepository;

        public DistributedCacheManager(IDistributedCacheRepository distributedCacheRepository)
        {
            this.distributedCacheRepository = distributedCacheRepository;
        }

        /// <summary>
        /// Gets the distributed cache manager.
        /// </summary>
        /// <value>
        /// The distributed cache manager.
        /// </value>
        public IDistributedCache distributedCache { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheManager" /> class.
        /// </summary>
        public DistributedCacheManager(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Result.</returns>
        public T Get<T>(string key)
        {
            return Common.Deserialize<T>(distributedCache.Get(key));
        }

        /// <summary>
        /// Checks the key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Result.</returns>
        public bool CheckKeyExists(string key)
        {
            Refresh(key);
            DistributeCacheProductTable distributeCacheTable = distributedCacheRepository.FindWithKey(string.Empty, string.Empty, key);

            return distributeCacheTable != null;
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns>Result.</returns>
        public T Set<T>(string key, T obj, double minutes = 10)
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(minutes),
                AbsoluteExpiration = DateTime.Now.AddMinutes(minutes),
            };

            return distributedCache.Set(key, obj, distributedCacheEntryOptions);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        public T Set<T>(string key, T obj)
        {
            return Set(key, obj, 10);
        }

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <param name="token">The token.</param>
        public async Task SetAsync<T>(string key, T obj, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            await distributedCache.SetAsync(key, obj, options, token);
        }

        public void Refresh(string key)
        {
            distributedCache.Refresh(key);
        }
    }
}
