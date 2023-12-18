// <copyright file="IDapperQueryParameters.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using Anhny010920.Expressions.Queries;
using DapperExtensions;

namespace Anhny010920.Core.Abstractions.Dapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDapperQueryParameters<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets or sets the sort expressions.
        /// </summary>
        /// <value>
        /// The sort expressions.
        /// </value>
        List<Tuple<IDapperQueryExpression<TEntity>, bool>> SortExpressions { get; set; }

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        List<Tuple<IDapperQueryExpression<TEntity>, Operator, object, int>> PredicatesExpressions { get; set; }

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        List<Tuple<GroupOperator, int>> Operators { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        IDbTransaction Transaction { get; set; }
    }
}
