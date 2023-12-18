// <copyright file="Anhny010290Dao.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Anhny010920.Core.Abstractions.Models.Response;
using Anhny010920.Core.Domain.Common;
using Anhny010920.Core.Enums;
using Anhny010920.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Z.BulkOperations;

namespace Anhny010920.Core.Abstractions.Queries
{
    public interface IEntityDao<TEntity> where TEntity : EntityBase, new()
    {
        EnumOptimisticConcurrency EnumOptimisticConcurrency { get; set; }
        bool HasChanges { get; }
        bool IsAutoDetectChangesEnabled { get; set; }
        bool IsNoTracking { get; set; }
        DbSet<TEntity> Table { get; }
        public DatabaseFacade Database { get; }

       // (string Schema, string TableName) TableSchemaAndName { get; }

        Task<DbContext> AddToContext(DbContextOptions dbContextOptions, TEntity entity, int count, int commitCount, bool recreateContext);
        Task<BulkOperationResultModel> BulkSaveChangesAsync(BulkTypes bulkTypes, bool persist = false, int batchTimeout = 120, CancellationToken cancellationToken = default);
        ITransactionResult Commit(List<Action> actions);
        ITransactionResult Commit(List<Action> actions, DbTransaction dbTransaction = null);
        Task<ITransactionResult> CommitAsync(List<Action> actions, DbTransaction dbTransaction = null);
        TEntity Delete(Guid id);
        void DeleteRange(List<Guid> listIds);
        int ExcuteSqlCommand(string command, params SqlParameter[] parameters);
        void ExecuteFuture();
        TEntity Find(Guid? id);
        TEntity FindAsNoTracking(Guid? id);
        Task<TEntity> FindAsNoTrackingAsync(Guid? id);
        Task<TEntity> FindAsync(Guid? id);
        TEntity FindIgnoreQueryFilters(Guid? id);
        Task<TEntity> FindIgnoreQueryFiltersAsync(Guid? id);
        TEntity FindWithKey(string includeReference = "", string includeCollection = "", params object[] para);
        TEntity FindWithKey(params object[] para);
        Task<TEntity> FindWithKeyAsync(string includeReference = "", string includeCollection = "", params object[] para);
        Task<TEntity> FindWithKeyAsync(params object[] para);
        Task<BulkOperationResultModel> FutureBulkAsync(List<TEntity> entities, BulkTypes bulkTypes, bool persist = false, int batchTimeout = 120, EFBulkOptions<TEntity> efBulkOptions = null, CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, object>> orderBy);
        IEnumerable<TEntity> GetAllEntities();
        IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false);
        IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false, bool asNoTracking = true);
        IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "");
        IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "", bool asNoTracking = true);
        IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        Task<List<TResult>> GetEntitiesResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null);
        Task<List<TResult>> GetEntitiesResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<List<TResult>> GetEntitiesResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, CancellationToken cancellationToken = default);
        Task<List<TResult>> GetEntitiesResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<List<TResult>> GetEntitiesResultAsync<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetEntitiesWithDataReader(string querySql, params SqlParameter[] parameters);
        Task<IEnumerable<TEntity>> GetEntitiesWithDataReaderAsync(string querySql, params SqlParameter[] parameters);
        TEntity GetEntity(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetEntity(Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        TEntity GetEntity(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        TEntity GetEntity(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default);
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<TEntity> GetEntityAsync(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        TResult GetEntityResult<TResult>(Expression<Func<TEntity, TResult>> selector = null);
        TResult GetEntityResult<TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null);
        TResult GetEntityResult<TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        TResult GetEntityResult<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        TResult GetEntityResult<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        Task<TResult> GetEntityResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null);
        Task<TResult> GetEntityResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<TResult> GetEntityResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, CancellationToken cancellationToken = default);
        Task<TResult> GetEntityResultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<TResult> GetEntityResultAsync<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetQuery();
        IQueryable<TEntity> GetQuery(bool isIncludeAll = false);
        IQueryable<TEntity> GetQuery(bool isIncludeAll = false, bool asNoTracking = true);
        IQueryable<TEntity> GetQuery(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        IQueryable<TEntity> GetQuery(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null);
        IQueryable<TEntity> GetQuery(string navigationPropertyPath = "");
        IQueryable<TEntity> GetQuery(string navigationPropertyPath = "", bool asNoTracking = true);
        IQueryable<TEntity> GetQuery(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true);
        IQueryable<TEntity> GetQuery(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, object>> include, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true);
        IQueryable<TResult> GetQuery<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true);
        IQueryable<TEntity> GetQueryWithRawSql(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true, params SqlParameter[] parameters);
        IQueryable<TEntity> GetQueryWithRawSql(string query, params SqlParameter[] parameters);
        IQueryable<TEntity> GetRangeAsync(IQueryable<TEntity> query, int skip, int take);
        void Insert(object entity);
        void Insert(TEntity entity);
        void InsertAsync(object entity);
        void InsertAsync(TEntity entity);
        void InsertOrUpdate(List<TEntity> entities);
        void InsertOrUpdate(TEntity modifiedEntity);
        Task InsertOrUpdateAsync(TEntity modifiedEntity);
        Task InsertRangeAsync(IEnumerable<TEntity> newEntities, bool isAutoDetectChangesEnabled = false);
        DbCommand LoadStoredProc(string storedProcName, DbTransaction dbTransaction = null);
        Task<PaginatedList<TResult>> PaginateAsync<TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null);
        Task<PaginatedList<TResult>> PaginateAsync<TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<PaginatedList<TResult>> PaginateAsync<TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null, CancellationToken cancellationToken = default);
        Task<PaginatedList<TResult>> PaginateAsync<TResult>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<PaginatedList<TResult>> PaginateAsync<TResult>(int pageIndex, int pageSize, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, TResult>> selector = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
        int SaveChanges(ref string messageError, ref Dictionary<string, string> differenceProperties);
        Task<int> SaveChangesAsync(string messageError, Dictionary<string, string> differenceProperties);
        void SoftDelete(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> newEntities, bool isAutoDetectChangesEnabled = false);
    }
}