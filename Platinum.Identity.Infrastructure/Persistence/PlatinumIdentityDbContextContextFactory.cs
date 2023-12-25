using Platinum.Core.Common;

namespace Platinum.Identity.Infrastructure.Persistence
{
    public class PlatinumIdentityDbContextContextFactory : DesignTimeDbContextFactoryBase<PlatinumIdentityDbContext>
    {
        public PlatinumIdentityDbContextContextFactory() : base("PlatinumIdentityDb")
        {
        }
    }
}
