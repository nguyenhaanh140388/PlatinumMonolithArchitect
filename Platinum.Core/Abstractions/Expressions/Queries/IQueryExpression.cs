// <copyright file="IQueryExpression.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System;
using System.Linq.Expressions;

namespace Platinum.Core.Abstractions.Expressions.Queries
{
    /// <summary>
    /// IQueryExpression.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryExpression<TEntity>
    {
        /// <summary>
        /// Ases the expression.
        /// </summary>
        /// <returns>Result.</returns>
        Expression<Func<TEntity, bool>> AsExpression();
    }
}
