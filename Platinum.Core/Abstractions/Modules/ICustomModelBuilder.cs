// <copyright file="ICustomModelBuilder.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace Anhny010920.Core.Abstractions.Modules
{
    /// <summary>
    /// ICustomModelBuilder.
    /// </summary>
    public interface ICustomModelBuilder
    {
        /// <summary>
        /// Builds the specified model builder.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        void Build(ModelBuilder modelBuilder);
    }
}
