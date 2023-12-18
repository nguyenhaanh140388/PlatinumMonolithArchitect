// <copyright file="IBaseRepository.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.Enums;
using Platinum.Core.Abstractions.Dapper;
using Platinum.Core.Abstractions.Queries;
using System;

namespace Platinum.Core.Abstractions.Repositories
{
    /// <summary>
    /// BaseRepository interface.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IBaseRepository : IDisposable
    {
        /// <summary>
        /// Gets or sets the entity query.
        /// </summary>
        /// <value>
        /// The entity query.
        /// </value>
        IEntityQuery EntityDao { get; set; }

        IDapper DapperDao { get; set; }

        /// <summary>
        /// Switches to data context.
        /// </summary>
        /// <param name="dataContextNames">The data context names.</param>
        void ToDbContext(DataContextNames dataContextNames);

        void ToDbConnection(DataContextNames dataContextNames);
    }
}
