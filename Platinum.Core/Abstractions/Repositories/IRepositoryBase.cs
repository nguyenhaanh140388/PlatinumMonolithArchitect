// <copyright file="IBaseRepository.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Dapper;
using Platinum.Core.Abstractions.Queries;
using Platinum.Core.Enums;

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
