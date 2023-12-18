using Anhny010920.Core.Abstractions.Queries;
using Anhny010920.Core.Domain.Common;

namespace Anhny010920.Core.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> : IEntityDao<TEntity> where TEntity : EntityBase, new()
    {
    }
}
