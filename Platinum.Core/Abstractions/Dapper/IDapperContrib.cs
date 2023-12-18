// <copyright file="IDapperContrib.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Platinum.Core.Abstractions.Dapper
{
    /// <summary>
    /// IDapperContrib.
    /// </summary>
    public interface IDapperContrib
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Result.</returns>
        IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : class, new();

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Result.</returns>
        TEntity Get<TEntity>(Guid id)
            where TEntity : class, new();

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        long Insert<TEntity>(TEntity obj)
            where TEntity : class, new();

        /// <summary>
        /// Inserts the specified list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Result.</returns>
        long Insert<TEntity>(IEnumerable<TEntity> list)
            where TEntity : class, new();

        /// <summary>
        /// Updates the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        bool Update<TEntity>(TEntity obj)
            where TEntity : class, new();

        /// <summary>
        /// Updates the specified list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Result.</returns>
        bool Update<TEntity>(IEnumerable<TEntity> list)
            where TEntity : class, new();

        /// <summary>
        /// Deletes the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        bool Delete<TEntity>(TEntity obj)
            where TEntity : class, new();

        /// <summary>
        /// Deletes the specified list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Result.</returns>
        bool Delete<TEntity>(IEnumerable<TEntity> list)
            where TEntity : class, new();

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Result.</returns>
        bool DeleteAll<TEntity>()
            where TEntity : class, new();
    }
}