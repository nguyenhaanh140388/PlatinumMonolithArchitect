using Platinum.Catalog.Core.Abstractions.Repositories;
using Platinum.Catalog.Core.Entities;
using Platinum.Catalog.Infrastructure.Repositories;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Abstractions.UnitOfWork;
using Platinum.Infrastructure.UnitOfWork;

namespace Platinum.Catalog.Infrastructure.UoW
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        public UnitOfWork(PlatinumCatalogContext context, IApplicationUserManager appUserManager) : base(context)
        {
            Categories = new CategoriesRepository(context, appUserManager);
        }

        public ICategoryRepository Categories { get; private set; }
    }
}
