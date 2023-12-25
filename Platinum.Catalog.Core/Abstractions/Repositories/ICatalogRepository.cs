using Platinum.Core.Abstractions.Repositories;
using Platinum.Core.Common;

namespace Platinum.Catalog.Core.Abstractions.Repositories
{
    public interface ICatalogRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntityBase, new()
    {
    }
}
