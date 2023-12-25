using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Platinum.Core.Abstractions.EntityFramework
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade DatabaseFacade { get;}
        void Dispose();
    }
}
