// <copyright file="IIncludeExpression.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq.Expressions;

namespace Anhny010920.Expressions.Queries
{
    public interface IIncludeExpression<TEntity>
    {
        Expression<Func<TEntity, object>> AsExpression();
    }
}
