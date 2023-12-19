// <copyright file="IDapper.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Dapper;
using System.Data;
using System.Data.Common;

namespace Platinum.Core.Abstractions.Dapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IDapper : IDisposable
    {
        /// <summary>
        /// Gets the dbconnection.
        /// </summary>
        /// <returns>Result.</returns>
        DbConnection GetDbconnection();

        /// <summary>
        /// Gets the specified sp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        TEntity GetEntity<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        List<TEntity> GetEntities<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Executes the specified sp.
        /// </summary>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Inserts the specified sp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        TEntity Insert<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Updates the specified sp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        TEntity Update<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        IEnumerable<TEntity> GetEntities<TEntity>(IDapperQueryParameters<TEntity> dapperQueryParameters)
            where TEntity : class, new();
    }
}
