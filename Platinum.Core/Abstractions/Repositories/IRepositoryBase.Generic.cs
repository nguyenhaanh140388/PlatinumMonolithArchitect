using Anhny010920.Core.Domain.Common;
using Platinum.Core.Abstractions.Queries;

namespace Platinum.Core.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> : IEntityDao<TEntity> where TEntity : EntityBase, new()
    {
    }
}
