using Platinum.Catalog.Core.Abstractions.Repositories;
using Platinum.Catalog.Core.Entities;
using Platinum.Core.Abstractions.Identitys;

namespace Platinum.Catalog.Infrastructure.Repositories
{
    public class CategoriesRepository : CatalogRepository<Category>, ICategoryRepository
    {
        public CategoriesRepository(
            PlatinumCatalogContext clientContext,
            IApplicationUserManager appUserManager = null
            )
            : base(clientContext, appUserManager)
        {
        }

        protected override void DisposeCore()
        {
            base.DisposeCore();
        }
    }
}
