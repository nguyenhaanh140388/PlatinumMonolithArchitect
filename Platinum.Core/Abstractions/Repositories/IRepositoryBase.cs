// <copyright file="IBaseRepository.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.Abstractions.Dapper;
using Anhny010920.Core.Abstractions.Queries;
using Anhny010920.Core.Enums;
using System;

namespace Anhny010920.Core.Abstractions.Repositories
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
