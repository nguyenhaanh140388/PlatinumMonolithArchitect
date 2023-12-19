using Microsoft.EntityFrameworkCore.Storage;
using Platinum.Infrastructure.Garbage;

namespace Platinum.Infrastructure.UnitOfWork
{

    public abstract class UnitOfWorkBase : Disposable, IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        public virtual Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync(default);
        }

        protected readonly IDbContext _dataContext;

        public UnitOfWorkBase(IDbContext context)
        {
            _dataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _dataContext.DatabaseFacade.BeginTransaction();
            return _transaction;
        }

        protected override void DisposeCore()
        {
            base.DisposeCore();
            _dataContext.Dispose();
            //if (_transaction != null)
            //    _transaction.Dispose();

            //_transaction = null;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
