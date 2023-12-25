// <copyright file="QueryBase.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Platinum.Core.Common;

namespace Platinum.Core.Dao.EF
{
    /// <summary>
    /// QueryBase.
    /// </summary>
    public abstract class QueryBase : Disposable
    {
        /// <summary>
        /// QueryBase.
        /// </summary>
        internal DbContext DbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBase"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected QueryBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
