using Microsoft.EntityFrameworkCore;
using Platinum.Catalog.Core.Entities;
using Platinum.Core.Common;

namespace Platinum.Identity.Infrastructure.Persistence
{
    public class PlatinumCatalogContextFactory : //IDesignTimeDbContextFactory<PlatinumCatalogContext> //
                                                DesignTimeDbContextFactoryBase<PlatinumCatalogContext> //
    {
        public PlatinumCatalogContextFactory()
            : base("PlatinumCatalog")
        {

        }

        //public PlatinumCatalogContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<PlatinumCatalogContext>();
        //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-HEOSBPL\NGUYENHAANH2021;Database=PlatinumCatalog;User Id= sa;Password=HaAnh@!@@$#$@$@!;TrustServerCertificate=true;");

        //    return new PlatinumCatalogContext(optionsBuilder.Options);
        //}
    }
}
