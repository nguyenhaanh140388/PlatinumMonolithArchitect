// <copyright file="QueryBase.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Platinum.Infrastructure.Garbage;

namespace Platinum.Infrastructure.Dao.EF
{
    /// <summary>
    /// QueryBase.
    /// </summary>
    public abstract class QueryBase : Disposable
    {
        /// <summary>
        /// anhny010920BlogDbContext.
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
