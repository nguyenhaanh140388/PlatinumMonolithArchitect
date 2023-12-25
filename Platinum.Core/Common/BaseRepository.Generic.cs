﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Abstractions.Repositories;
using Platinum.Core.Common;
using Platinum.Core.Dao.EF;

namespace Platinum.Infrastructure.Repositories
{
    public abstract class BaseRepository<TDbContext, TEntity> : EntityDao<TEntity>, IBaseRepository<TEntity> 
        where TEntity : EntityBase, new() 
        where TDbContext : DbContext
    {
        protected TDbContext _context;

        public BaseRepository(TDbContext context, IApplicationUserManager userManager = null) 
            :base(context, userManager)
        {
            _context = context;
        }

        protected IEnumerable<EntityEntry> ListEntries => _context.ChangeTracker.Entries();
    }
}
