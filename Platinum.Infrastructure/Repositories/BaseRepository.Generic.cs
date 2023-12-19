using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Platinum.Infrastructure.Dao.EF;

namespace Platinum.Infrastructure.Repositories
{
    public abstract class BaseRepository<TDbContext, TEntity> : EntityDao<TEntity>, IBaseRepository<TEntity> where TEntity : EntityBase, new() where TDbContext : DbContext
    {
        protected TDbContext _context;

        public BaseRepository(TDbContext context, IAppUserManager appUserManager = null) :
            base(context, appUserManager)
        {
            _context = context;
        }

        protected IEnumerable<EntityEntry> ListEntries => _context.ChangeTracker.Entries();
    }
}
