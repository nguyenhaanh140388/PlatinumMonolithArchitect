using Platinum.Core.Abstractions.Queries;
using Platinum.Core.Common;

namespace Platinum.Core.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> : IEntityDao<TEntity> where TEntity : EntityBase, new()
    {
    }
}
