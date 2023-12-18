// <copyright file="IModuleInitializer.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Modules
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
