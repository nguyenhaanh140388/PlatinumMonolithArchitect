// <copyright file="IDaoFactory.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Dapper;
using Platinum.Core.Abstractions.Queries;
using Platinum.Core.Enums;

namespace Platinum.Core.Abstractions.Dao
{
    /// <summary>
    /// IDaoFactory.
    /// </summary>
    public interface IDaoFactory
    {
        /// <summary>
        /// Gets the entity framework DAO.
        /// </summary>
        /// <param name="dataContextNames">The data context names.</param>
        /// <returns>IEntityQuery.</returns>
        IEntityQuery GetEntityFrameworkDao(DataContextNames dataContextNames);

        IDapper GetDapperDao(DataContextNames dataContextNames);
    }
}
