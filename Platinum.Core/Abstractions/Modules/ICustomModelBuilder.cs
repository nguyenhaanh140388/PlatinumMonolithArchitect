// <copyright file="ICustomModelBuilder.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace Platinum.Core.Abstractions.Modules
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
