// <copyright file="IMemoryCacheManager.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.State
{
    public interface IMemoryCacheManager
    {
        T Get<T>(string key);

        // T Set<T>(string key, T obj, Action<object, object, EvictionReason, object> action, double minutes = 10);
        T Set<T>(string key, T obj, double minutes = 10);

        T Set<T>(string key, T obj);
    }
}