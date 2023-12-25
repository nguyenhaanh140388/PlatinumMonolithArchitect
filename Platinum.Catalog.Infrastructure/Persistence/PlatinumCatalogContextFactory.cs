using Platinum.Catalog.Core.Entities;
using Platinum.Core.Common;

namespace Platinum.Identity.Infrastructure.Persistence
{
    public class PlatinumCatalogContextFactory :DesignTimeDbContextFactoryBase<PlatinumCatalogContext> 
    {
        public PlatinumCatalogContextFactory()
            : base("PlatinumCatalog")
        {

        }
    }
}
