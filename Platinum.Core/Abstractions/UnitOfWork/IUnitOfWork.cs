using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.UnitOfWork
{
    public interface IUnitOfWork : IDisposable //<U> where U : DbContext, IDisposable
    {
        IDbContextTransaction BeginTransaction();
        void Commit();
        void Rollback();
        Task<int> SaveChangesAsync();
    }
}
