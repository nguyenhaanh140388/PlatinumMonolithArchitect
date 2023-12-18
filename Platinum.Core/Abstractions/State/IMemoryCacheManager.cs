// <copyright file="IMemoryCacheManager.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
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