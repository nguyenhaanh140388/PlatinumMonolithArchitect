// <copyright file="DapperQueryParameters.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using DapperExtensions;
using Platinum.Core.Abstractions.Dapper;
using Platinum.Core.Abstractions.Expressions.Queries;
using System.Data;

namespace Platinum.Infrastructure.Dao.Dappers
{
    /// <summary>
    /// DapperQueryParameters.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="IDapperQueryParameters{TEntity}" />
    /// <seealso cref="Interfaces.Queries.IDapperQueryParameters" />
    public class DapperQueryParameters<TEntity> : IDapperQueryParameters<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets or sets the sort expressions.
        /// </summary>
        /// <value>
        /// The sort expressions.
        /// </value>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public List<Tuple<IDapperQueryExpression<TEntity>, bool>> SortExpressions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public List<Tuple<IDapperQueryExpression<TEntity>, Operator, object, int>> PredicatesExpressions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public List<Tuple<GroupOperator, int>> Operators { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IDbTransaction Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
