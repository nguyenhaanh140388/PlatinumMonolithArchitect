using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Platinum.Identity.Infrastructure.Persistence
{
    public class PlatinumIdentityDbContextContextFactory : IDesignTimeDbContextFactory<PlatinumIdentityDbContext> //DesignTimeDbContextFactoryBase<Anhny010920CatalogContext> //
    {
        public PlatinumIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlatinumIdentityDbContext>();
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-HEOSBPL\NGUYENHAANH2021;Database=PlatinumIdentityDb;User Id= sa;Password=HaAnh@!@@$#$@$@!;TrustServerCertificate=true");

            return new PlatinumIdentityDbContext(optionsBuilder.Options);
        }
    }
}
