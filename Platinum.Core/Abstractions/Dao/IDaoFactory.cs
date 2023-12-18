// <copyright file="IDaoFactory.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.Abstractions.Dapper;
using Anhny010920.Core.Abstractions.Queries;
using Anhny010920.Core.Enums;

namespace Anhny010920.Core.Abstractions.Dao
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
