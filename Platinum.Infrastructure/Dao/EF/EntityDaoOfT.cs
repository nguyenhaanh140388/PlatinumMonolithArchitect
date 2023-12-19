// <copyright file="Anhny010290Dao.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Abstractions.Models.Response;
using Platinum.Core.Abstractions.Queries;
using Platinum.Core.Common;
using Platinum.Core.Enums;
using Platinum.Core.Extensions;
using Platinum.Core.Models;
using Platinum.Core.Utils;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Z.BulkOperations;
using Z.EntityFramework.Extensions;

namespace Platinum.Infrastructure.Dao.EF
{

    /// <summary>
    /// EntityQuey.
    /// </summary>
    /// <seealso cref="QueryBase" />
    /// <seealso cref="IEntityQuery" />
    public class EntityDao<TEntity> : QueryBase, IEntityDao<TEntity> where TEntity : EntityBase, new()
    {
        /// <summary>
        /// The is no tracking.
        /// </summary>
        private bool isNoTracking = false;
        protected readonly IAppUserManager appUserManager;

        public DbSet<TEntity> Table { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDao" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public EntityDao(DbContext dbContext, IAppUserManager appUserManager = null)
            : base(dbContext)
        {
            // Using a constructor that requires optionsBuilder (EF Core) 
            EntityFrameworkManager.ContextFactory = context =>
            {
                return dbContext;
            };

            this.appUserManager = appUserManager;
            Table = dbContext.Set<TEntity>();

        }

        public DatabaseFacade Database => DbContext.Database;

        protected override void DisposeCore()
        {
            if (DbContext != null)
                DbContext.Dispose();
        }

        /// <summary>
        /// Gets or sets a value indicating whether isNoTracking.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is no tracking; otherwise, <c>false</c>.
        /// </value>
        public bool IsNoTracking
        {
            get
            {
                return isNoTracking;
            }

            set
            {
                isNoTracking = value;
                if (isNoTracking)
                {
                    DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                }
            }
        }

        /// <summary>
        /// Gets or sets enumOptimisticConcurrency.
        /// </summary>
        /// <value>
        /// The enum optimistic concurrency.
        /// </value>
        public EnumOptimisticConcurrency EnumOptimisticConcurrency { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isAutoDetectChangesEnabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is automatic detect changes enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutoDetectChangesEnabled { get; set; }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        public void SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
            DbContext.Entry(entity).Property(x => x.IsDeleted).IsModified = true;
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        public void SoftDelete(Guid id)
        {
            TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            PropertyInfo prop = entity.GetType().GetProperty(nameof(entity.Id));
            prop.SetValue(entity, id, null);

            DbContext.Entry(entity).Property(x => x.IsDeleted).IsModified = true;
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void SoftDeleteRange(List<Guid> listIds)
        {
            for (int i = 0; i < listIds.Count; i++)
            {
                SoftDelete(listIds[0]);
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        public TEntity Delete(Guid id)
        {
            TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            PropertyInfo prop = entity.GetType().GetProperty(nameof(entity.Id));
            prop.SetValue(entity, id, null);
            DbContext.Remove(entity);
            return entity;
            //this.DbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteRange(List<Guid> listIds)
        {
            for (int i = 0; i < listIds.Count; i++)
            {
                Delete(listIds[i]);
            }
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert(object entity)
        {
            DbContext.Add(entity);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertAsync(object entity)
        {
            DbContext.AddAsync(entity);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert(TEntity entity)
        {
            Table.Add(entity);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertAsync(TEntity entity)
        {
            Table.AddAsync(entity);
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void InsertOrUpdate(List<TEntity> entities)
        {
            // string errorMessage = string.Empty;
            // Dictionary<string, string> differenceProperties = null;
            DbSet<TEntity> dbSetEntity = Table;

            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));
            IKey primaryKey = entityType.FindPrimaryKey();
            object[] keyValues = new object[primaryKey.Properties.Count];

            foreach (TEntity modifiedEntity in entities)
            {
                for (int i = 0; i < keyValues.Length; i++)
                {
                    keyValues[i] = primaryKey.Properties[i].GetGetter().GetClrValue(modifiedEntity);
                }

                TEntity entityDb = dbSetEntity.Find(keyValues);

                if (entityDb == null)
                {
                    dbSetEntity.Add(modifiedEntity);
                }
                else
                {
                    // can take any object and maps property values to the attached entity based on the property name.
                    DbContext.Entry(entityDb).CurrentValues.SetValues(modifiedEntity);
                }
            }

            // return this.SaveChanges(ref errorMessage, ref differenceProperties);
        }

        public async Task<DbContext> AddToContext(DbContextOptions dbContextOptions, TEntity entity, int count, int commitCount, bool recreateContext)
        {
            await DbContext.AddAsync(entity);

            if (count % commitCount == 0)
            {
                await DbContext.SaveChangesAsync();

                if (recreateContext)
                {
                    DbContext.Dispose();
                    DbContext = new DbContext(dbContextOptions);
                    DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                }
            }

            return DbContext;
        }
        //private Action<BulkOperation<TEntity>> BulkOptionsDefault(ResultInfo resultInfo,
        //    List<AuditEntry> auditEntries,
        //    EFBulkOptions<TEntity> efBulkOptions,
        //    BulkTypes bulkTypes,
        //    int batchTimeout)
        //{
        //    if (efBulkOptions is null)
        //    {
        //        throw new ArgumentNullException(nameof(efBulkOptions));
        //    }

        //    return options =>
        //{
        //    options.BatchTimeout = batchTimeout;
        //    options.UseRowsAffected = true;
        //    options.ResultInfo = resultInfo;
        //    options.UseAudit = true;
        //    options.AuditEntries = auditEntries;

        //    switch (bulkTypes)
        //    {
        //        case BulkTypes.Insert:
        //            options.CopyPropertiesFrom(efBulkOptions.BulkInsertOptions);
        //            break;
        //        case BulkTypes.Update:
        //            options.CopyPropertiesFrom(efBulkOptions.BulkUpdateOptions);
        //            break;
        //        case BulkTypes.Merge:
        //            options.CopyPropertiesFrom(efBulkOptions.BulkMergeOptions);
        //            break;
        //        case BulkTypes.Synchronize:
        //            options.CopyPropertiesFrom(efBulkOptions.BulkSynchronizeOptions);
        //            break;
        //        default:
        //            break;
        //    }
        //};
        //}

        private Action<TEntity> AuditBulk(BulkTypes bulkTypes) => i =>
        {
            var userAuditId = appUserManager?.CurrentUserId;

            switch (bulkTypes)
            {
                case BulkTypes.Insert:
                    EntityFrameworkManager.PreBulkInsert = (ctx, obj) =>
                    {
                        if (obj is IEnumerable<TEntity>)
                        {
                            foreach (var entity in (IEnumerable<TEntity>)obj)
                            {
                                entity.CreatedDate = DateTime.Now;
                                entity.CreatedBy = userAuditId;
                            }
                        }
                    };
                    break;

                case BulkTypes.Update:
                    EntityFrameworkManager.PreBulkUpdate = (ctx, obj) =>
                    {
                        if (obj is IEnumerable<TEntity>)
                        {
                            foreach (var entity in (IEnumerable<TEntity>)obj)
                            {
                                entity.ModifiedDate = DateTime.Now;
                                entity.ModifiedBy = userAuditId;
                            }
                        }
                    };
                    break;
                case BulkTypes.SoftDelete:
                    EntityFrameworkManager.PreBulkUpdate = (ctx, obj) =>
                    {
                        var list = obj as IEnumerable<TEntity>;

                        foreach (var entity in list)
                        {
                            entity.IsDeleted = true;
                            entity.ModifiedDate = DateTime.Now;
                            entity.ModifiedBy = userAuditId;
                        }

                        ctx.BulkUpdateAsync(list);

                        ((List<TEntity>)obj).Clear();
                    };
                    break;

                case BulkTypes.Synchronize:
                case BulkTypes.Merge:
                    EntityFrameworkManager.PreBulkSynchronize = EntityFrameworkManager.PreBulkMerge = (ctx, obj) =>
                    {
                        if (obj is IEnumerable<TEntity>)
                        {
                            foreach (var entity in (IEnumerable<TEntity>)obj)
                            {
                                if (entity.Id == Guid.Empty || entity.Id == null)
                                {
                                    entity.CreatedDate = DateTime.Now;
                                    entity.CreatedBy = userAuditId;
                                }
                                else
                                {
                                    entity.ModifiedDate = DateTime.Now;
                                    entity.ModifiedBy = userAuditId;
                                }
                            }
                        }
                    };

                    break;
                case BulkTypes.SaveChanges:
                    EntityFrameworkManager.PreBulkSaveChanges = ctx =>
                    {
                        var modifiedEntities = ctx.ChangeTracker.Entries().ToList();

                        foreach (var entity in modifiedEntities)
                        {
                            if (entity.State == EntityState.Added)
                            {
                                entity.CurrentValues["CreatedDate"] = DateTime.Now;
                                entity.CurrentValues["CreatedBy"] = userAuditId;
                            }
                            else if (entity.State == EntityState.Modified)
                            {
                                entity.CurrentValues["ModifiedDate"] = DateTime.Now;
                                entity.CurrentValues["ModifiedBy"] = userAuditId;
                            }
                        }
                    };
                    break;
                default:
                    break;
            }
        };

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="tableName">Name of the table.</param>
        //public async Task<BulkOperationResultModel> FutureBulkAsync(List<TEntity> entities,
        //    BulkTypes bulkTypes,
        //    bool persist = false,
        //    int batchTimeout = 120,
        //    EFBulkOptions<TEntity> efBulkOptions = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    var resultInfo = new Z.BulkOperations.ResultInfo();
        //    var auditEntries = new List<AuditEntry>();
        //    var checkedBulkOptions = BulkOptionsDefault(resultInfo,
        //        auditEntries,
        //        efBulkOptions,
        //        bulkTypes,
        //        batchTimeout);

        //    AuditBulk(bulkTypes);

        //    switch (bulkTypes)
        //    {
        //        case BulkTypes.Insert:
        //            //DbContext.FutureAction
        //            // (async x =>
        //            // {
        //            await DbContext.BulkInsertAsync(entities,
        //                checkedBulkOptions,
        //                cancellationToken);
        //            // });

        //            break;
        //        case BulkTypes.Update:
        //            // DbContext.FutureAction
        //            // (async x =>
        //            // {
        //            await DbContext.BulkUpdateAsync(entities,
        //                checkedBulkOptions,
        //                cancellationToken);
        //            //  });

        //            break;
        //        case BulkTypes.SoftDelete:
        //        case BulkTypes.Delete:
        //            // DbContext.FutureAction
        //            // (async x =>
        //            // {
        //            await DbContext.BulkDeleteAsync(entities,
        //                checkedBulkOptions,
        //                cancellationToken);
        //            //  });

        //            break;
        //        case BulkTypes.Merge:
        //            // DbContext.FutureAction
        //            //  (async x =>
        //            //  {
        //            await DbContext.BulkMergeAsync(entities,
        //                checkedBulkOptions,
        //                cancellationToken);
        //            //  });

        //            break;
        //        case BulkTypes.Synchronize:
        //            // DbContext.FutureAction
        //            // (async x =>
        //            // {
        //            await DbContext.BulkSynchronizeAsync(entities,
        //                checkedBulkOptions,
        //                cancellationToken);
        //            // });

        //            break;
        //        default:
        //            break;
        //    };

        //    if (persist)
        //    {
        //        //this.DbContext.ExecuteFutureAction(true);
        //    }

        //    return new BulkOperationResultModel()
        //    {
        //        AuditEntries = auditEntries,
        //        ResultInfo = resultInfo
        //    };
        //}

        public void ExecuteFuture()
        {
            DbContext.ExecuteFutureAction(true);
        }

        private Action<BulkOperation> BulkOptions(ResultInfo resultInfo,
            List<AuditEntry> auditEntries,
            BulkTypes bulkTypes,
            int batchTimeout) =>
            options =>
            {
                options.BatchTimeout = batchTimeout;
                options.UseRowsAffected = true;
                options.ResultInfo = resultInfo;
                options.UseAudit = true;
                options.AuditEntries = auditEntries;

                var userAuditId = appUserManager?.CurrentUserId;
                EntityFrameworkManager.PreBulkSaveChanges = ctx =>
                {
                    var modifiedEntities = ctx.ChangeTracker.Entries().ToList();

                    foreach (var entity in modifiedEntities)
                    {
                        if (entity.State == EntityState.Added)
                        {
                            entity.CurrentValues["CreatedDate"] = DateTime.Now;
                            entity.CurrentValues["CreatedBy"] = userAuditId;
                        }
                        else if (entity.State == EntityState.Modified)
                        {
                            entity.CurrentValues["ModifiedDate"] = DateTime.Now;
                            entity.CurrentValues["ModifiedBy"] = userAuditId;
                        }
                    }
                };
            };

        public async Task<BulkOperationResultModel> BulkSaveChangesAsync(BulkTypes bulkTypes,
            bool persist = false,
            int batchTimeout = 120,
            CancellationToken cancellationToken = default)
        {
            var resultInfo = new Z.BulkOperations.ResultInfo();
            var auditEntries = new List<AuditEntry>();
            var checkedBulkOptions = BulkOptions(resultInfo,
                auditEntries,
                bulkTypes,
                batchTimeout);

            AuditBulk(bulkTypes);

            await DbContext.BulkSaveChangesAsync(checkedBulkOptions, cancellationToken);
            return new BulkOperationResultModel()
            {
                AuditEntries = auditEntries,
                ResultInfo = resultInfo
            };
        }

        /// <inheritdoc/>
        public async Task InsertOrUpdateAsync(TEntity modifiedEntity)
        {
            DbSet<TEntity> dbSetEntity = Table;

            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));
            IKey primaryKey = entityType.FindPrimaryKey();
            object[] keyValues = new object[primaryKey.Properties.Count];

            for (int i = 0; i < keyValues.Length; i++)
            {
                keyValues[i] = primaryKey.Properties[i].GetGetter().GetClrValue(modifiedEntity);
            }

            TEntity entityDb = await dbSetEntity.FindAsync(keyValues);
            var userAuditId = appUserManager?.CurrentUserId;
            if (entityDb == null)
            {
                if (modifiedEntity is EntityBase entityBase)
                {
                    entityBase.CreatedBy = userAuditId;
                    entityBase.ModifiedBy = userAuditId;
                    entityBase.CreatedDate = DateTime.Now;
                    entityBase.ModifiedDate = DateTime.Now;
                }

                await dbSetEntity.AddAsync(modifiedEntity);
            }
            else
            {
                if (modifiedEntity is EntityBase entityBase)
                {
                    entityBase.ModifiedBy = userAuditId;
                    entityBase.ModifiedDate = DateTime.Now;
                }

                // can take any object and maps property values to the attached entity based on the property name.
                DbContext.Entry(entityDb).CurrentValues.SetValues(modifiedEntity);
            }

            // return this.SaveChanges(ref errorMessage, ref differenceProperties);
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="modifiedEntity">The modified entity.</param>
        public void InsertOrUpdate(TEntity modifiedEntity)

        {
            DbSet<TEntity> dbSetEntity = Table;
            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));
            IKey primaryKey = entityType.FindPrimaryKey();
            object[] keyValues = new object[primaryKey.Properties.Count];

            for (int i = 0; i < keyValues.Length; i++)
            {
                keyValues[i] = primaryKey.Properties[i].GetGetter().GetClrValue(modifiedEntity);
            }

            TEntity entityDb = dbSetEntity.Find(keyValues);
            var userAuditId = appUserManager?.CurrentUserId;
            if (entityDb == null)
            {
                if (modifiedEntity is EntityBase entityBase)
                {
                    entityBase.CreatedBy = userAuditId;
                    entityBase.ModifiedBy = userAuditId;
                    entityBase.CreatedDate = DateTime.Now;
                    entityBase.ModifiedDate = DateTime.Now;
                }

                dbSetEntity.Add(modifiedEntity);
            }
            else
            {
                if (modifiedEntity is EntityBase entityBase)
                {
                    entityBase.ModifiedBy = userAuditId;
                    entityBase.ModifiedDate = DateTime.Now;
                }

                // can take any object and maps property values to the attached entity based on the property name.
                DbContext.Entry(entityDb).CurrentValues.SetValues(modifiedEntity);
            }

            // return this.SaveChanges(ref errorMessage, ref differenceProperties);
        }

        /// <summary>
        /// SaveChangesInContext.
        /// </summary>
        /// <param name="isAutoDetectChangesEnabled">if set to <c>true</c> [is automatic detect changes enabled].</param>
        /// <param name="newEntities">The new entities.</param>
        public async Task InsertRangeAsync(IEnumerable<TEntity> newEntities, bool isAutoDetectChangesEnabled = false)
        {
            if (isAutoDetectChangesEnabled)
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            }

            await Table.AddRangeAsync(newEntities);

            if (isAutoDetectChangesEnabled)
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        public void UpdateRange(IEnumerable<TEntity> newEntities, bool isAutoDetectChangesEnabled = false)
        {
            if (isAutoDetectChangesEnabled)
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            }

            Table.UpdateRange(newEntities);

            if (isAutoDetectChangesEnabled)
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }


        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(string messageError, Dictionary<string, string> differenceProperties)
        {
            try
            {
                int effectNumber = await DbContext.SaveChangesAsync();
                return effectNumber;
            }
            catch (Exception exception)
            {
                if (exception is DbUpdateConcurrencyException dbUpdateConcurrencyException)
                {
                    try
                    {
                        switch (EnumOptimisticConcurrency)
                        {
                            case EnumOptimisticConcurrency.ClientWins:
                                await dbUpdateConcurrencyException.Entries.Single().ReloadAsync();
                                return await DbContext.SaveChangesAsync();
                            case EnumOptimisticConcurrency.DatabaseWins:
                                var entry = dbUpdateConcurrencyException.Entries.Single();
                                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                return await DbContext.SaveChangesAsync();
                            default:
                                foreach (EntityEntry entityEntry in dbUpdateConcurrencyException.Entries)
                                {
                                    PropertyValues trackedEntry = entityEntry.GetDatabaseValues();
                                    object entityFromDatabase = trackedEntry.ToObject();
                                    object entityFromUser = entityEntry.Entity;

                                    bool isConcurrency = ObjectUtils.CompareObject(entityFromUser, entityFromDatabase, out differenceProperties);
                                    if (!isConcurrency)
                                    {
                                        messageError = "The record was modified by another user after the page loaded\n" +
                                            "The save operation was cancel and the updated database values were display.\n" +
                                            "If you still want to edit this record, please save again.";

                                        // Timestamp = entityFromDatabase.Timestamp;
                                    }
                                }

                                return 0;
                        }
                    }
                    catch (Exception updateException)
                    {
                        ExceptionDispatchInfo.Capture(updateException.InnerException).Throw();
                        return 0;
                    }
                }

                return 0;
            }
        }

        /// <summary>
        /// SaveChangesInContext.
        /// </summary>
        /// <param name="messageError">messageError.</param>
        /// <param name="differenceProperties">differenceProperties.</param>
        /// <returns>
        /// The record number.
        /// </returns>
        public int SaveChanges(ref string messageError, ref Dictionary<string, string> differenceProperties)
        {
            try
            {
                int effectNumber = DbContext.SaveChanges();
                return effectNumber;
            }
            catch (Exception exception)
            {
                if (exception is DbUpdateConcurrencyException dbUpdateConcurrencyException)
                {
                    try
                    {
                        switch (EnumOptimisticConcurrency)
                        {
                            case EnumOptimisticConcurrency.ClientWins:
                                dbUpdateConcurrencyException.Entries.Single().Reload();
                                return DbContext.SaveChanges();
                            case EnumOptimisticConcurrency.DatabaseWins:
                                var entry = dbUpdateConcurrencyException.Entries.Single();
                                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                return DbContext.SaveChanges();
                            default:
                                foreach (EntityEntry entityEntry in dbUpdateConcurrencyException.Entries)
                                {
                                    PropertyValues trackedEntry = entityEntry.GetDatabaseValues();
                                    object entityFromDatabase = trackedEntry.ToObject();
                                    object entityFromUser = entityEntry.Entity;

                                    bool isConcurrency = ObjectUtils.CompareObject(entityFromUser, entityFromDatabase, out differenceProperties);
                                    if (!isConcurrency)
                                    {
                                        messageError = "The record was modified by another user after the page loaded\n" +
                                            "The save operation was cancel and the updated database values were display.\n" +
                                            "If you still want to edit this record, please save again.";

                                        // Timestamp = entityFromDatabase.Timestamp;
                                    }
                                }

                                return 0;
                        }
                    }
                    catch (Exception updateException)
                    {
                        messageError = updateException.Message;
                        ExceptionDispatchInfo.Capture(updateException.InnerException).Throw();
                        return 0;
                    }
                }

                messageError = exception.Message;

                return 0;
            }
        }

        /// <summary>
        /// Commit data with list action and cross-context transaction.
        /// </summary>
        /// <param name="actions">The list actions.</param>
        /// <param name="dbTransaction">The cross-context transaction.</param>
        /// <returns>
        /// The effectNumber has one or more a record.
        /// </returns>
        public ITransactionResult Commit(List<Action> actions, DbTransaction dbTransaction = null)
        {
            string messageError = string.Empty;
            Dictionary<string, string> differenceProperties = null;
            ITransactionResult transactionResult = new TransactionResult
            {
                EffectedRecord = 0,
            };

            using (var transaction = dbTransaction == null ? DbContext.Database.BeginTransaction() : DbContext.Database.UseTransaction(dbTransaction))
            {
                try
                {
                    foreach (Action action in actions)
                    {
                        action.Invoke();
                    }

                    int effectNumber = SaveChanges(ref messageError, ref differenceProperties); // TODO

                    if (effectNumber > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }

                    transactionResult.MessageError = messageError;
                    transactionResult.EffectedRecord = effectNumber;
                    transactionResult.DifferenceProperties = differenceProperties;
                }
                catch (Exception exception)
                {
                    transactionResult.MessageError = exception.Message;
                    transaction.Rollback();
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                }

                return transactionResult;
            }
        }

        /// <summary>
        /// Commit data with list action and cross-context transaction.
        /// </summary>
        /// <param name="actions">The list actions.</param>
        /// <param name="dbTransaction">The cross-context transaction.</param>
        /// <returns>
        /// Effected record.
        /// </returns>
        public async Task<ITransactionResult> CommitAsync(List<Action> actions, DbTransaction dbTransaction = null)
        {
            string messageError = string.Empty;
            Dictionary<string, string> differenceProperties = null;
            ITransactionResult transactionResult = new TransactionResult
            {
                EffectedRecord = 0,
            };

            using (var transaction = dbTransaction == null ? await DbContext.Database.BeginTransactionAsync() : DbContext.Database.UseTransaction(dbTransaction))
            {
                try
                {
                    foreach (Action action in actions)
                    {
                        action.Invoke();
                    }

                    int effectNumber = await SaveChangesAsync(messageError, differenceProperties); // TODO

                    if (effectNumber > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }

                    transactionResult.MessageError = messageError;
                    transactionResult.EffectedRecord = effectNumber;
                    transactionResult.DifferenceProperties = differenceProperties;
                }
                catch (Exception exception)
                {
                    transactionResult.Exception = exception;
                    transactionResult.MessageError = exception.Message;
                    transaction.Rollback();
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                }

                return transactionResult;
            }
        }

        /// <summary>
        /// Commits the specified actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// Effected record.
        /// </returns>
        public ITransactionResult Commit(List<Action> actions)
        {
            string messageError = string.Empty;
            Dictionary<string, string> differenceProperties = null;
            ITransactionResult transactionResult = new TransactionResult
            {
                EffectedRecord = 0,
            };

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Action action in actions)
                    {
                        action.Invoke();
                    }

                    int effectNumber = SaveChanges(ref messageError, ref differenceProperties); // TODO

                    if (effectNumber > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }

                    transactionResult.MessageError = messageError;
                    transactionResult.EffectedRecord = effectNumber;
                    transactionResult.DifferenceProperties = differenceProperties;
                }
                catch (Exception exception)
                {
                    transactionResult.Exception = exception;
                    transactionResult.MessageError = exception.Message;
                    transaction.Rollback();
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                }

                return transactionResult;
            }
        }

        /// <summary>
        /// FindWithKey.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="para">The list object.</param>
        /// <returns>
        /// The single object of TEntity.
        /// </returns>
        /// <exception cref="ArgumentNullException">para - The parameter can not be null.</exception>
        /// <exception cref="ArgumentNullException">para - The parameter can not be null.</exception>
        public TEntity FindWithKey(params object[] para)

        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            return FindWithKey(string.Empty, string.Empty, para);
        }

        /// <inheritdoc/>
        public async Task<TEntity> FindWithKeyAsync(params object[] para)

        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            return await FindWithKeyAsync(string.Empty, string.Empty, para);
        }

        public async Task<TEntity> FindWithKeyAsync(string includeReference = "", string includeCollection = "", params object[] para)

        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            TEntity entity = await Table.FindAsync(para);

            if (includeReference != null)
            {
                foreach (var includeProperty in includeReference.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    await DbContext.Entry(entity).Reference(includeProperty).LoadAsync();
                }
            }

            if (includeCollection != null)
            {
                foreach (var includeProperty in includeCollection.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    await DbContext.Entry(entity).Collection(includeProperty).LoadAsync();
                }
            }

            return entity;
        }

        public TEntity Find(Guid? id) => Table.Find(id);
        public TEntity FindAsNoTracking(Guid? id) => Table.Where(x => x.Id == id).AsNoTracking().FirstOrDefault();
        public TEntity FindIgnoreQueryFilters(Guid? id) => Table.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);

        public async Task<TEntity> FindAsync(Guid? id) => await Table.FindAsync(id);
        public async Task<TEntity> FindAsNoTrackingAsync(Guid? id) => await Table.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<TEntity> FindIgnoreQueryFiltersAsync(Guid? id) => await Table.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        public IEnumerable<TEntity> GetAll() => Table;

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, object>> orderBy)
            => Table.OrderBy(orderBy);

        public IQueryable<TEntity> GetRangeAsync(IQueryable<TEntity> query, int skip, int take)
            => query.Skip(skip).Take(take);

        //public (string Schema, string TableName) TableSchemaAndName
        //{
        //    get
        //    {
        //        var metaData = DbContext.Model
        //            .FindEntityType(typeof(TEntity).FullName)
        //            .SqlServer();
        //        return (metaData.Schema, metaData.TableName);
        //    }
        //}

        public bool HasChanges => DbContext.ChangeTracker.HasChanges();


        /// <summary>
        /// Finds the with key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="includeReference">The include reference.</param>
        /// <param name="includeCollection">The include collection.</param>
        /// <param name="para">The para.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentNullException">para - The parameter can not be null.</exception>
        /// <exception cref="ArgumentNullException">para - The parameter can not be null.</exception>
        public TEntity FindWithKey(string includeReference = "", string includeCollection = "", params object[] para)

        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            TEntity entity = Table.Find(para);

            if (includeReference != null)
            {
                foreach (var includeProperty in includeReference.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    DbContext.Entry(entity).Reference(includeProperty).Load();
                }
            }

            if (includeCollection != null)
            {
                foreach (var includeProperty in includeCollection.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    DbContext.Entry(entity).Collection(includeProperty).Load();
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true)

        {
            IQueryable<TEntity> query = GetQuery(isIncludeAll, filter, orderBy, asNoTracking);
            return query.AsEnumerable();
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)

        {
            return GetAllEntities(false, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false, bool asNoTracking = true)

        {
            return GetAllEntities(isIncludeAll, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities(bool isIncludeAll = false)

        {

            return GetAllEntities(isIncludeAll, false);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities()

        {
            return GetAllEntities(false);
        }

        /// <summary>
        /// Gets all entities with include.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true)

        {
            IQueryable<TEntity> query = GetQuery(navigationPropertyPath, filter, orderBy, asNoTracking);
            return query.AsEnumerable();
        }

        /// <summary>
        /// Gets all entities with include.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)

        {
            return GetAllEntitiesWithInclude(navigationPropertyPath, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities with include.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "", bool asNoTracking = true)

        {
            return GetAllEntitiesWithInclude(navigationPropertyPath, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities with include.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesWithInclude(string navigationPropertyPath = "")

        {
            return GetAllEntitiesWithInclude(navigationPropertyPath, false);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Result.</returns>
        public TResult GetEntityResult<TResult>(Expression<Func<TEntity, TResult>> selector = null)
        {
            return GetEntityResult(selector, null);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Result.</returns>
        public TResult GetEntityResult<TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null)
        {
            return GetEntityResult(selector, filter, false);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TResult GetEntityResult<TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)
        {
            return GetEntityResult(null, selector, filter, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TResult GetEntityResult<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)
        {
            return GetEntityResult(navigationPropertyPath, selector, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TResult GetEntityResult<TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true)
        {
            return GetQuery(navigationPropertyPath, filter, orderBy, asNoTracking)
                .Select(selector)
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = true)

        {
            return GetQuery(navigationPropertyPath, filter, orderBy, asNoTracking)
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)

        {
            return GetEntity(navigationPropertyPath, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity(Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)

        {
            return GetEntity(null, filter, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity(Expression<Func<TEntity, bool>> filter = null)

        {
            return GetEntity(filter, false);
        }

        /// <summary>
        /// GetSource.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="navigationPropertyPath">The list include.</param>
        /// <param name="filter">The Expression filer.</param>
        /// <param name="orderBy">The Order.</param>
        /// <param name="asNoTracking">Check is tracking.</param>
        /// <returns>
        /// IQueryable.
        /// </returns>
        public IQueryable<TEntity> GetQuery(
            string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true)

        {
            IQueryable<TEntity> query = Table;

            if (navigationPropertyPath != null)
            {
                foreach (var includeProperty in navigationPropertyPath.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        /// <summary>Gets the source.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public IQueryable<TResult> GetQuery<TResult>(
            string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = true)

        {
            return GetQuery(navigationPropertyPath, filter, orderBy, asNoTracking)
                .Select(selector);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
             int pageIndex,
             int pageSize,
             string navigationPropertyPath = "",
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Expression<Func<TEntity, TResult>> selector = null,
             bool asNoTracking = true,
             CancellationToken cancellationToken = default)

        {
            return await GetQuery(navigationPropertyPath, filter, orderBy, selector, asNoTracking).ToPaginatedList(pageIndex, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
              Expression<Func<TEntity, TResult>> selector = null,
              bool asNoTracking = true,
              CancellationToken cancellationToken = default)

        {
            return await PaginateAsync(pageIndex, pageSize, string.Empty, filter, orderBy, selector, asNoTracking, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Expression<Func<TEntity, TResult>> selector = null,
              bool asNoTracking = true,
              CancellationToken cancellationToken = default)

        {
            return await PaginateAsync(pageIndex, pageSize, filter, null, selector, asNoTracking, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Expression<Func<TEntity, TResult>> selector = null,
              CancellationToken cancellationToken = default)

        {
            return await PaginateAsync(pageIndex, pageSize, filter, selector, true, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Expression<Func<TEntity, TResult>> selector = null)

        {
            return await PaginateAsync(pageIndex, pageSize, filter, selector, true, default);
        }

        public DbCommand LoadStoredProc(string storedProcName, DbTransaction dbTransaction = null)
        {
            var cmd = DbContext.Database.GetDbConnection().CreateCommand();
            cmd.Transaction = dbTransaction ?? DbContext.Database.CurrentTransaction?.GetDbTransaction();
            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        /// <summary>Gets the source asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<TResult>> GetEntitiesResultAsync<TResult>(
            string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)

        {
            return await GetQuery(navigationPropertyPath, filter, orderBy, selector, asNoTracking).ToListAsync(cancellationToken);
        }

        /// <summary>Gets the source asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<TResult>> GetEntitiesResultAsync<TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)

        {
            return await GetEntitiesResultAsync(string.Empty, filter, orderBy, selector, asNoTracking, cancellationToken);
        }

        /// <summary>Gets the source asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<TResult>> GetEntitiesResultAsync<TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)

        {
            return await GetEntitiesResultAsync(filter, null, selector, asNoTracking, cancellationToken);
        }

        /// <summary>Gets the source asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task<List<TResult>> GetEntitiesResultAsync<TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null,
            CancellationToken cancellationToken = default)

        {
            return GetEntitiesResultAsync(filter, null, selector, true, cancellationToken);
        }

        /// <summary>Gets the source asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<TResult>> GetEntitiesResultAsync<TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null)

        {
            return await GetEntitiesResultAsync(filter, null, selector, true, default);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TEntity> GetEntityAsync(
          string navigationPropertyPath = "",
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          bool asNoTracking = true,
          CancellationToken cancellationToken = default)

        {
            return await GetQuery(navigationPropertyPath, filter, orderBy, asNoTracking)
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TEntity> GetEntityAsync(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          bool asNoTracking = true,
          CancellationToken cancellationToken = default)

        {
            return await GetEntityAsync(string.Empty, filter, orderBy, asNoTracking, cancellationToken);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TEntity> GetEntityAsync(
          Expression<Func<TEntity, bool>> filter = null,
          bool asNoTracking = true,
          CancellationToken cancellationToken = default)

        {
            return await GetEntityAsync(filter, null, asNoTracking, cancellationToken);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TEntity> GetEntityAsync(
          Expression<Func<TEntity, bool>> filter = null,
          CancellationToken cancellationToken = default)

        {
            return await GetEntityAsync(filter, true, cancellationToken);
        }

        public async Task<TEntity> GetEntityAsync(
            Expression<Func<TEntity, bool>> filter = null)

        {
            return await GetEntityAsync(filter, true);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TResult> GetEntityResultAsync<TResult>(
          string navigationPropertyPath = "",
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          Expression<Func<TEntity, TResult>> selector = null,
          bool asNoTracking = true,
          CancellationToken cancellationToken = default)

        {
            return await GetQuery(navigationPropertyPath, filter, orderBy, asNoTracking)
                .Select(selector)
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TResult> GetEntityResultAsync<TResult>(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          Expression<Func<TEntity, TResult>> selector = null,
          bool asNoTracking = true,
          CancellationToken cancellationToken = default)

        {
            return await GetEntityResultAsync(string.Empty, filter, orderBy, selector, asNoTracking, cancellationToken);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TResult> GetEntityResultAsync<TResult>(
          Expression<Func<TEntity, bool>> filter = null,
          Expression<Func<TEntity, TResult>> selector = null,
          bool asNoTracking = true,
          CancellationToken cancellationToken = default)

        {
            return await GetEntityResultAsync(filter, null, selector, asNoTracking, cancellationToken);
        }

        /// <summary>Gets the entity asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TResult> GetEntityResultAsync<TResult>(
          Expression<Func<TEntity, bool>> filter = null,
          Expression<Func<TEntity, TResult>> selector = null,
          CancellationToken cancellationToken = default)

        {
            return await GetEntityResultAsync(filter, selector, true, cancellationToken);
        }

        public async Task<TResult> GetEntityResultAsync<TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null)

        {
            return await GetEntityResultAsync(filter, selector, default);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = true)

        {
            return GetQuery(navigationPropertyPath, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(string navigationPropertyPath = "", bool asNoTracking = true)

        {
            return GetQuery(navigationPropertyPath, null, asNoTracking);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(string navigationPropertyPath = "")

        {
            return GetQuery(navigationPropertyPath, true);
        }

        /// <summary>
        /// GetSource.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The Expression filer.</param>
        /// <param name="orderBy">The Order.</param>
        /// <param name="asNoTracking">Check is tracking.</param>
        /// <returns>
        /// IQueryable.
        /// </returns>
        public IQueryable<TEntity> GetQuery(
            bool isIncludeAll = false,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true)

        {
            IQueryable<TEntity> query = Table;

            if (isIncludeAll)
            {
                query = query.Include(DbContext.GetIncludePaths(typeof(TEntity)));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(bool isIncludeAll = false,
            Expression<Func<TEntity, bool>> filter = null,
            bool asNoTracking = true)
        {
            return GetQuery(isIncludeAll, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(bool isIncludeAll = false,
            bool asNoTracking = true)
        {
            return GetQuery(isIncludeAll, null, asNoTracking);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(bool isIncludeAll = false)
        {
            return GetQuery(isIncludeAll, true);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery()
        {
            return GetQuery(true);
        }


        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQuery(false, filter, true);
        }

        /// <summary>
        /// GetWithRawSql.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="query">The Query.</param>
        /// <param name="parameters">The Parameters.</param>
        /// <returns>
        /// IEnumerable.
        /// </returns>
        /// <exception cref="ArgumentException">The parameter from query and parameter no equal.</exception>
        public IQueryable<TEntity> GetQueryWithRawSql(string query, params SqlParameter[] parameters)

        {
            if (parameters == null)
            {
                return Table
              .FromSqlRaw(query);
            }
            else
            {
                MatchCollection matchedParameterNames = Regex.Matches(query, "@[a-zA-Z]+");

                if (matchedParameterNames.Count != parameters.Count())
                {
                    throw new ArgumentException("The parameter from query and parameter no equal.");
                }

                List<SqlParameter> sqlParameters = parameters.GetSqlParameters(matchedParameterNames);
                return Table
                    .FromSqlRaw(query, sqlParameters.ToArray());
            }
        }

        /// <summary>
        /// Gets the source with raw SQL.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetQueryWithRawSql(string querySql, string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true,
            params SqlParameter[] parameters)
        {
            IQueryable<TEntity> queryEntity = GetQueryWithRawSql(querySql, parameters);

            if (navigationPropertyPath != null)
            {
                foreach (var includeProperty in navigationPropertyPath.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryEntity = queryEntity.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                queryEntity = queryEntity.Where(filter);
            }

            if (asNoTracking)
            {
                queryEntity = queryEntity.AsNoTracking();
            }

            if (orderBy != null)
            {
                return orderBy(queryEntity);
            }

            return queryEntity;
        }

        /// <summary>
        /// GetSource.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="include">The include.</param>
        /// <param name="filter">The Expression filer.</param>
        /// <param name="orderBy">The Order.</param>
        /// <param name="asNoTracking">Check is tracking.</param>
        /// <returns>
        /// IQueryable.
        /// </returns>
        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, object>> include,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = true)

        {
            IQueryable<TEntity> query = Table;

            if (include != null)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithDataReader(string querySql, params SqlParameter[] parameters)

        {
            RelationalDataReader relationalDataReader = DbContext.Database.ExecuteSqlQuery(querySql, parameters);

            DbDataReader dbDataReader = relationalDataReader.DbDataReader;
            while (dbDataReader.Read())
            {
                TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));

                for (int row = 0; row < dbDataReader.FieldCount; row++)
                {
                    Type type = typeof(TEntity);
                    PropertyInfo prop = type.GetProperty(dbDataReader.GetName(row));
                    prop.SetValue(entity, dbDataReader.GetValue(row), null);
                }

                yield return entity;
            }

            relationalDataReader.Dispose();
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public async Task<IEnumerable<TEntity>> GetEntitiesWithDataReaderAsync(string querySql, params SqlParameter[] parameters)
        {
            RelationalDataReader relationalDataReader = await DbContext.Database.ExecuteSqlQueryAsync(querySql, default, parameters);

            DbDataReader dbDataReader = relationalDataReader.DbDataReader;

            List<TEntity> lists = new List<TEntity>();

            while (await dbDataReader.ReadAsync())
            {
                TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));

                for (int row = 0; row < dbDataReader.FieldCount; row++)
                {
                    Type type = typeof(TEntity);
                    PropertyInfo prop = type.GetProperty(dbDataReader.GetName(row));
                    prop.SetValue(entity, dbDataReader.GetValue(row), null);
                }

                // yield return entity;
                lists.Add(entity);
            }

            relationalDataReader.Dispose();
            return lists;
        }

        /// <summary>
        /// Excutes the SQL command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentException">The parameter from query and parameter no equal.</exception>
        /// <exception cref="ArgumentException">The parameter from query and parameter no equal.</exception>
        public int ExcuteSqlCommand(string command, params SqlParameter[] parameters)
        {
            if (parameters == null)
            {
                return 0;
            }
            else
            {
                MatchCollection matchedParameterNames = Regex.Matches(command, "@[a-zA-Z]+");

                if (matchedParameterNames.Count != parameters.Count())
                {
                    throw new ArgumentException("The parameter from query and parameter no equal.");
                }

                List<SqlParameter> sqlParameters = parameters.GetSqlParameters(matchedParameterNames);
                return DbContext.Database.ExecuteSqlRaw(command, sqlParameters.ToArray());
            }
        }
    }
}
