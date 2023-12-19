using Platinum.Infrastructure.Data.EntityFramework;

namespace Platinum.Infrastructure.Repositories
{
    public class DistributedCacheRepository : RepositoryBase<Anhny010920AdministratorContext, DistributeCacheProductTable>, IDistributedCacheRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        public DistributedCacheRepository(Anhny010920AdministratorContext clientContext)
         : base(clientContext)
        {
        }
    }
}
