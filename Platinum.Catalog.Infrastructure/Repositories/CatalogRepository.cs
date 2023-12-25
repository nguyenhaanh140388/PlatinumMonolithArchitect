using Platinum.Catalog.Core.Abstractions.Repositories;
using Platinum.Catalog.Core.Entities;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Common;
using Platinum.Infrastructure.Repositories;

namespace Platinum.Catalog.Infrastructure.Repositories
{
    public class CatalogRepository<TEntity>
        : BaseRepository<PlatinumCatalogContext, TEntity>, ICatalogRepository<TEntity> where TEntity : EntityBase, new()
    {
        private static IApplicationUserManager userManager;

        public CatalogRepository(PlatinumCatalogContext clientContext, IApplicationUserManager appUserManager = null) :
            base(clientContext, appUserManager)
        {
            userManager = appUserManager;
        }

    }
}
