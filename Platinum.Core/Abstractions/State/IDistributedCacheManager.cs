// <copyright file="IDistributedCacheManager.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.Extensions.Caching.Distributed;

namespace Platinum.Core.Abstractions.State
{
    /// <summary>
    /// IDistributedCacheManager.
    /// </summary>
    public interface IDistributedCacheManager
    {
        /// <summary>
        /// Checks the key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Result.</returns>
        bool CheckKeyExists(string key);

        /// <summary>
        /// Refreshes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Refresh(string key);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Result.</returns>
        TObject Get<TObject>(string key);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        TObject Set<TObject>(string key, TObject obj);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns>Result.</returns>
        TObject Set<TObject>(string key, TObject obj, double minutes = 10);

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task SetAsync<TObject>(string key, TObject obj, DistributedCacheEntryOptions options, CancellationToken token = default);
    }
}