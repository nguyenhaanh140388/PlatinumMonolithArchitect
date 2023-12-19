// <copyright file="IEntityQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Models.Response;
using Platinum.Core.Common;
using Platinum.Core.Enums;
using System.Data.Common;
using System.Linq.Expressions;

namespace Platinum.Core.Abstractions.Queries
{
    /// <summary>
    /// IEntityQuery.
    /// </summary>
    public interface IEntityQuery
    {
        EnumOptimisticConcurrency EnumOptimisticConcurrency { get; set; }

        bool IsAutoDetectChangesEnabled { get; set; }

        bool IsNoTracking { get; set; }

        void BulkInsert<TEntity>(List<TEntity> entities, string tableName) where TEntity : class, new();

        ITransactionResult Commit(List<Action> actions);

        ITransactionResult Commit(List<Action> actions, DbTransaction dbTransaction = null);

        Task<ITransactionResult> CommitAsync(List<Action> actions, DbTransaction dbTransaction = null);

        void Delete<TEntity>(Guid id);

        void Delete<TEntity>(List<Guid> listIds);

        Task DeleteAsync<TEntity>(Guid id);

        Task DeleteAsync<TEntity>(List<Guid> listIds);

        void Dispose();

        int ExcuteSqlCommand(string command, params SqlParameter[] parameters);

        TEntity FindWithKey<TEntity>(string includeReference = "", string includeCollection = "", params object[] para) where TEntity : class, new();

        TEntity FindWithKey<TEntity>(params object[] para) where TEntity : class, new();

        Task<TEntity> FindWithKeyAsync<TEntity>(string includeReference = "", string includeCollection = "", params object[] para) where TEntity : class, new();

        Task<TEntity> FindWithKeyAsync<TEntity>(params object[] para) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntities<TEntity>() where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>() where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false, bool asNoTracking = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "") where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "", bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>() where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "") where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "", bool asNoTracking = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null) where TEntity : class, new();

        Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql) where TEntity : class, new();

        IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, string navigationPropertyPath = "", bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IEnumerable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, params SqlParameter[] parameters) where TEntity : class, new();

        IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "") where TEntity : class, new();

        IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", params SqlParameter[] parameters) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetEntitiesWithRawSqlAsync<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetEntitiesWithRawSqlAsync<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        TResult GetEntity<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null) where TEntity : class, new();

        TResult GetEntity<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        TResult GetEntity<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        TResult GetEntity<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, new();

        TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        TEntity GetEntity<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        TEntity GetEntity<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        Task<TResult> GetEntityAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<TResult> GetEntityAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<TResult> GetEntityAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<TResult> GetEntityAsync<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        IQueryable<TResult> GetSource<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "") where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "", bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSource<TEntity>(Expression<Func<TEntity, object>> include, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false) where TEntity : class, new();

        IQueryable<TEntity> GetSourceWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters) where TEntity : class, new();

        IQueryable<TEntity> GetSourceWithRawSql<TEntity>(string query, params SqlParameter[] parameters) where TEntity : class, new();

        void Insert(bool isAutoDetectChangesEnabled, IEnumerable<object> newEntities);

        void Insert(object entity);

        Task InsertAsync(bool isAutoDetectChangesEnabled, IEnumerable<object> newEntities);

        Task InsertAsync(object entity);

        void InsertOrUpdate<TEntity>(List<TEntity> entities) where TEntity : class, new();

        void InsertOrUpdate<TEntity>(TEntity modifiedEntity) where TEntity : class, new();

        Task InsertOrUpdateAsync<TEntity>(List<TEntity> entities) where TEntity : class, new();

        Task InsertOrUpdateAsync<TEntity>(TEntity modifiedEntity) where TEntity : class, new();

        Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null) where TEntity : class, new();

        Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(int pageIndex, int pageSize, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = false, CancellationToken cancellationToken = default) where TEntity : class, new();

        int SaveChanges(ref string messageError, ref Dictionary<string, string> differenceProperties);

        Task<int> SaveChangesAsync(string messageError, Dictionary<string, string> differenceProperties);
    }
}