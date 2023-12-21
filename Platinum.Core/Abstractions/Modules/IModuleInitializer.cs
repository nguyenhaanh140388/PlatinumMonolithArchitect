// <copyright file="IModuleInitializer.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Platinum.Core.Abstractions.Modules
{
    /// <summary>
    /// IModuleInitializer.
    /// </summary>
    public interface IModuleInitializer
    {
        /// <summary>
        /// Initializes the specified service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        void Init(IServiceCollection serviceCollection,
             ContainerBuilder container,
            IConfiguration config);

        Task AutoTasks(IServiceProvider serviceProvider);
    }
}
