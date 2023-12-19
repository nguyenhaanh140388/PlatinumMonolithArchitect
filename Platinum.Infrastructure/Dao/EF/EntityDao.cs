// <copyright file="Anhny010290Dao.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace Platinum.Infrastructure.Dao.EF
{
    /// <summary>
    /// EntityQuey.
    /// </summary>
    /// <seealso cref="QueryBase" />
    /// <seealso cref="IEntityQuery" />
    public class EntityDao : QueryBase, IEntityQuery
    {
        /// <summary>
        /// The is no tracking.
        /// </summary>
        private bool isNoTracking = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDao" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public EntityDao(DbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeCore()
        {
            base.DisposeCore();
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
        /// DeleteAsync.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public Task DeleteAsync<TEntity>(Guid id)
        {
            return Task.Run(() =>
            {
                Delete<TEntity>(id);
            });
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        public void Delete<TEntity>(Guid id)
        {
            TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            PropertyInfo prop = entity.GetType().GetProperty("Id");
            prop.SetValue(entity, id, null);
            DbContext.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="listIds">The list ids.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task DeleteAsync<TEntity>(List<Guid> listIds)
        {
            return Task.Run(() =>
            {
                Delete<TEntity>(listIds);
            });
        }

        /// <summary>
        /// Deletes the specified list ids.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="listIds">The list ids.</param>
        public void Delete<TEntity>(List<Guid> listIds)
        {
            foreach (Guid id in listIds)
            {
                Delete<TEntity>(id);
            }
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="isAutoDetectChangesEnabled">if set to <c>true</c> [is automatic detect changes enabled].</param>
        /// <param name="newEntities">The new entities.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task InsertAsync(bool isAutoDetectChangesEnabled, IEnumerable<object> newEntities)
        {
            return Task.Run(() =>
            {
                Insert(isAutoDetectChangesEnabled, newEntities);
            });
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task InsertAsync(object entity)
        {
            return Task.Run(() =>
            {
                Insert(entity);
            });
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
        /// Inserts the or update asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task InsertOrUpdateAsync<TEntity>(List<TEntity> entities)
            where TEntity : class, new()
        {
            return Task.Run(() =>
            {
                InsertOrUpdate(entities);
            });
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void InsertOrUpdate<TEntity>(List<TEntity> entities)
            where TEntity : class, new()
        {
            // string errorMessage = string.Empty;
            // Dictionary<string, string> differenceProperties = null;
            DbSet<TEntity> dbSetEntity = DbContext.Set<TEntity>();

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

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BulkInsert<TEntity>(List<TEntity> entities, string tableName)
            where TEntity : class, new()
        {
            try
            {
                //// Using a constructor that requires optionsBuilder (EF Core) 
                // EntityFrameworkManager.ContextFactory = context =>
                // {
                //    var optionsBuilder = new DbContextOptionsBuilder<dbContext>();
                //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-G4DPCAH;Database=Anhny010920Catalog;User Id= sa; Password=123;");
                //    return new dbContext(optionsBuilder.Options);
                // };

                // this.dbContext.BulkInsert(entities, options =>
                // {
                //    options.IncludeGraph = true;
                // });
            }
            catch (Exception)
            {
            }
        }

        /// <inheritdoc/>
        public async Task InsertOrUpdateAsync<TEntity>(TEntity modifiedEntity)
          where TEntity : class, new()
        {
            DbSet<TEntity> dbSetEntity = DbContext.Set<TEntity>();

            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));
            IKey primaryKey = entityType.FindPrimaryKey();
            object[] keyValues = new object[primaryKey.Properties.Count];

            for (int i = 0; i < keyValues.Length; i++)
            {
                keyValues[i] = primaryKey.Properties[i].GetGetter().GetClrValue(modifiedEntity);
            }

            TEntity entityDb = await dbSetEntity.FindAsync(keyValues);

            if (entityDb == null)
            {
                if (modifiedEntity is EntityBase entityBase)
                {
                    entityBase.CreatedDate = DateTime.Now;
                    entityBase.ModifiedDate = DateTime.Now;
                }

                dbSetEntity.Add(modifiedEntity);
            }
            else
            {
                if (modifiedEntity is EntityBase entityBase)
                {
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
        public void InsertOrUpdate<TEntity>(TEntity modifiedEntity)
            where TEntity : class, new()
        {
            DbSet<TEntity> dbSetEntity = DbContext.Set<TEntity>();

            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));
            IKey primaryKey = entityType.FindPrimaryKey();
            object[] keyValues = new object[primaryKey.Properties.Count];

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

            // return this.SaveChanges(ref errorMessage, ref differenceProperties);
        }

        /// <summary>
        /// SaveChangesInContext.
        /// </summary>
        /// <param name="isAutoDetectChangesEnabled">if set to <c>true</c> [is automatic detect changes enabled].</param>
        /// <param name="newEntities">The new entities.</param>
        public void Insert(bool isAutoDetectChangesEnabled, IEnumerable<object> newEntities)
        {
            if (isAutoDetectChangesEnabled)
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            }

            DbContext.AddRange(newEntities);

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
        public TEntity FindWithKey<TEntity>(params object[] para)
            where TEntity : class, new()
        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            return FindWithKey<TEntity>(string.Empty, string.Empty, para);
        }

        /// <inheritdoc/>
        public async Task<TEntity> FindWithKeyAsync<TEntity>(params object[] para)
         where TEntity : class, new()
        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            return await FindWithKeyAsync<TEntity>(string.Empty, string.Empty, para);
        }

        public async Task<TEntity> FindWithKeyAsync<TEntity>(string includeReference = "", string includeCollection = "", params object[] para)
     where TEntity : class, new()
        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            TEntity entity = await DbContext.Set<TEntity>().FindAsync(para);

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
        public TEntity FindWithKey<TEntity>(string includeReference = "", string includeCollection = "", params object[] para)
        where TEntity : class, new()
        {
            if (para == null)
            {
                throw new ArgumentNullException(nameof(para), "The parameter can not be null");
            }

            TEntity entity = DbContext.Set<TEntity>().Find(para);

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
        public IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(isIncludeAll, filter, orderBy, asNoTracking);
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
        public IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
            where TEntity : class, new()
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
        public IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false, bool asNoTracking = false)
            where TEntity : class, new()
        {
            return GetAllEntities<TEntity>(isIncludeAll, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities<TEntity>(bool isIncludeAll = false)
            where TEntity : class, new()
        {

            return GetAllEntities<TEntity>(isIncludeAll, false);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntities<TEntity>()
            where TEntity : class, new()
        {
            return GetAllEntities<TEntity>(false);
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
        public IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(navigationPropertyPath, filter, orderBy, asNoTracking);
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
        public IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
            where TEntity : class, new()
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
        public IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "", bool asNoTracking = false)
            where TEntity : class, new()
        {
            return GetAllEntitiesWithInclude<TEntity>(navigationPropertyPath, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities with include.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesWithInclude<TEntity>(string navigationPropertyPath = "")
           where TEntity : class, new()
        {
            return GetAllEntitiesWithInclude<TEntity>(navigationPropertyPath, false);
        }

        /// <summary>
        /// Gets all entities with include asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            return Task.Run(() =>
            {
                return GetAllEntitiesWithInclude(navigationPropertyPath, filter, orderBy, asNoTracking);
            });
        }

        /// <summary>
        /// Gets all entities with include asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
         where TEntity : class, new()
        {
            return GetAllEntitiesWithIncludeAsync(navigationPropertyPath, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities with include asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "", bool asNoTracking = false)
            where TEntity : class, new()
        {
            return GetAllEntitiesWithIncludeAsync<TEntity>(navigationPropertyPath, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities with include asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>(string navigationPropertyPath = "")
            where TEntity : class, new()
        {
            return GetAllEntitiesWithIncludeAsync<TEntity>(navigationPropertyPath, false);
        }

        /// <summary>
        /// Gets all entities with include asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesWithIncludeAsync<TEntity>()
            where TEntity : class, new()
        {
            return GetAllEntitiesWithIncludeAsync<TEntity>(string.Empty);
        }

        /// <summary>
        /// Gets all entities asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            return Task.Run(() =>
            {
                return GetAllEntities(isIncludeAll, filter, orderBy, asNoTracking);
            });
        }

        /// <summary>
        /// Gets all entities asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
        where TEntity : class, new()
        {
            return GetAllEntitiesAsync(isIncludeAll, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false, bool asNoTracking = false)
        where TEntity : class, new()
        {
            return GetAllEntitiesAsync<TEntity>(isIncludeAll, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(bool isIncludeAll = false)
            where TEntity : class, new()
        {
            return GetAllEntitiesAsync<TEntity>(isIncludeAll, false);
        }

        /// <summary>
        /// Gets all entities asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>()
         where TEntity : class, new()
        {
            return GetAllEntitiesAsync<TEntity>(false);
        }

        /// <summary>
        /// Gets the entities with raw SQL asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetEntitiesWithRawSqlAsync<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return Task.Run(() =>
            {
                return GetEntitiesWithRawSql(querySql, navigationPropertyPath, filter, orderBy, asNoTracking, parameters);
            });
        }

        /// <summary>
        /// Gets the entities with raw SQL asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task<IEnumerable<TEntity>> GetEntitiesWithRawSqlAsync<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false, params SqlParameter[] parameters)
        where TEntity : class, new()
        {
            return GetEntitiesWithRawSqlAsync(querySql, navigationPropertyPath, filter, null, asNoTracking, parameters);
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
        public TResult GetEntity<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
  where TEntity : class, new()
        {
            return GetEntity(navigationPropertyPath, selector, filter, null, asNoTracking);
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
        public TResult GetEntity<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
where TEntity : class, new()
        {
            return GetEntity(null, selector, filter, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Result.</returns>
        public TResult GetEntity<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null)
where TEntity : class, new()
        {
            return GetEntity(selector, filter, false);
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
        public TResult GetEntity<TEntity, TResult>(string navigationPropertyPath = "", Expression<Func<TEntity, TResult>> selector = null, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
        where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(navigationPropertyPath, filter, orderBy, asNoTracking);

            return query.Select(selector)
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
        public TEntity GetEntity<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(navigationPropertyPath, filter, orderBy, asNoTracking);

            return query.SingleOrDefault();
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
where TEntity : class, new()
        {
            return GetEntity<TEntity>(navigationPropertyPath, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
where TEntity : class, new()
        {
            return GetEntity(null, filter, asNoTracking);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns>Result.</returns>
        public TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, new()
        {
            return GetEntity(filter, false);
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
        public IQueryable<TEntity> GetSource<TEntity>(
            bool isIncludeAll = false,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

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
        public IQueryable<TEntity> GetSource<TEntity>(
            string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

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
        public IQueryable<TResult> GetSource<TEntity, TResult>(
            string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = false)
           where TEntity : class, new()
        {
            return GetSource(navigationPropertyPath, filter, orderBy, asNoTracking).Select(selector);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(
             int pageIndex,
             int pageSize,
             string navigationPropertyPath = "",
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Expression<Func<TEntity, TResult>> selector = null,
             bool asNoTracking = false,
             CancellationToken cancellationToken = default)
             where TEntity : class, new()
        {
            return await GetSource(navigationPropertyPath, filter, orderBy, selector, asNoTracking).ToPaginatedList(pageIndex, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
              Expression<Func<TEntity, TResult>> selector = null,
              bool asNoTracking = false,
              CancellationToken cancellationToken = default)
              where TEntity : class, new()
        {
            return await PaginateAsync(pageIndex, pageSize, string.Empty, filter, orderBy, selector, asNoTracking, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Expression<Func<TEntity, TResult>> selector = null,
              bool asNoTracking = false,
              CancellationToken cancellationToken = default)
              where TEntity : class, new()
        {
            return await PaginateAsync(pageIndex, pageSize, filter, null, selector, asNoTracking, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Expression<Func<TEntity, TResult>> selector = null,
              CancellationToken cancellationToken = default)
              where TEntity : class, new()
        {
            return await PaginateAsync(pageIndex, pageSize, filter, selector, true, cancellationToken);
        }

        public async Task<PaginatedList<TResult>> PaginateAsync<TEntity, TResult>(
              int pageIndex,
              int pageSize,
              Expression<Func<TEntity, bool>> filter = null,
              Expression<Func<TEntity, TResult>> selector = null)
              where TEntity : class, new()
        {
            return await PaginateAsync(pageIndex, pageSize, filter, selector, true, default);
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
        public async Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(
            string navigationPropertyPath = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
            where TEntity : class, new()
        {
            return await GetSource(navigationPropertyPath, filter, orderBy, selector, asNoTracking).ToListAsync(cancellationToken);
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
        public async Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
            where TEntity : class, new()
        {
            return await GetEntitiesAsync(string.Empty, filter, orderBy, selector, asNoTracking, cancellationToken);
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
        public async Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
            where TEntity : class, new()
        {
            return await GetEntitiesAsync(filter, null, selector, asNoTracking, cancellationToken);
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
        public Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null,
            CancellationToken cancellationToken = default)
            where TEntity : class, new()
        {
            return GetEntitiesAsync(filter, null, selector, true, cancellationToken);
        }

        /// <summary>Gets the source asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<TResult>> GetEntitiesAsync<TEntity, TResult>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TResult>> selector = null)
            where TEntity : class, new()
        {
            return await GetEntitiesAsync(filter, null, selector, true, default);
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
        public async Task<TResult> GetEntityAsync<TEntity, TResult>(
          string navigationPropertyPath = "",
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          Expression<Func<TEntity, TResult>> selector = null,
          bool asNoTracking = false,
          CancellationToken cancellationToken = default)
          where TEntity : class, new()
        {
            return await GetSource(navigationPropertyPath, filter, orderBy, asNoTracking)
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
        public async Task<TResult> GetEntityAsync<TEntity, TResult>(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          Expression<Func<TEntity, TResult>> selector = null,
          bool asNoTracking = false,
          CancellationToken cancellationToken = default)
          where TEntity : class, new()
        {
            return await GetEntityAsync(string.Empty, filter, orderBy, selector, asNoTracking, cancellationToken);
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
        public async Task<TResult> GetEntityAsync<TEntity, TResult>(
          Expression<Func<TEntity, bool>> filter = null,
          Expression<Func<TEntity, TResult>> selector = null,
          bool asNoTracking = false,
          CancellationToken cancellationToken = default)
          where TEntity : class, new()
        {
            return await GetEntityAsync(filter, null, selector, asNoTracking, cancellationToken);
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
        public async Task<TResult> GetEntityAsync<TEntity, TResult>(
          Expression<Func<TEntity, bool>> filter = null,
          Expression<Func<TEntity, TResult>> selector = null,
          CancellationToken cancellationToken = default)
          where TEntity : class, new()
        {
            return await GetEntityAsync(filter, selector, true, cancellationToken);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(navigationPropertyPath, filter, null, asNoTracking);

            return query;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "", bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource<TEntity>(navigationPropertyPath, null, asNoTracking);

            return query;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetSource<TEntity>(string navigationPropertyPath = "")
          where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource<TEntity>(navigationPropertyPath, false);

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
        public IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(isIncludeAll, filter, null, asNoTracking);

            return query;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(isIncludeAll, null, orderBy, asNoTracking);

            return query;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="isIncludeAll">if set to <c>true</c> [is include all].</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetSource<TEntity>(bool isIncludeAll = false, bool asNoTracking = false)
             where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource<TEntity>(isIncludeAll, null, null, asNoTracking);

            return query;
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
        public IQueryable<TEntity> GetSourceWithRawSql<TEntity>(string query, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            if (parameters == null)
            {
                return DbContext.Set<TEntity>()
              .FromSqlRaw(query).AsQueryable();
            }
            else
            {
                MatchCollection matchedParameterNames = Regex.Matches(query, "@[a-zA-Z]+");

                if (matchedParameterNames.Count != parameters.Count())
                {
                    throw new ArgumentException("The parameter from query and parameter no equal.");
                }

                List<SqlParameter> sqlParameters = parameters.GetSqlParameters(matchedParameterNames);
                return DbContext.Set<TEntity>()
                    .FromSqlRaw(query, sqlParameters.ToArray()).AsQueryable();
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
        public IQueryable<TEntity> GetSourceWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters)
    where TEntity : class, new()
        {
            IQueryable<TEntity> queryEntity = GetSourceWithRawSql<TEntity>(querySql, parameters);

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
        public IQueryable<TEntity> GetSource<TEntity>(Expression<Func<TEntity, object>> include, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

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
        /// Gets all entities include expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="inlude">The inlude.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSource(inlude, filter, orderBy, asNoTracking);
            return query.AsEnumerable();
        }

        /// <summary>
        /// Gets all entities include expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="inlude">The inlude.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude, Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false)
            where TEntity : class, new()
        {
            return GetAllEntitiesIncludeExpression(inlude, filter, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities include expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="inlude">The inlude.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude, bool asNoTracking = false)
        where TEntity : class, new()
        {
            return GetAllEntitiesIncludeExpression(inlude, null, asNoTracking);
        }

        /// <summary>
        /// Gets all entities include expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="inlude">The inlude.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetAllEntitiesIncludeExpression<TEntity>(Expression<Func<TEntity, object>> inlude)
    where TEntity : class, new()
        {
            return GetAllEntitiesIncludeExpression(inlude, false);
        }

        /// <summary>
        /// Gets the entities with raw SQL.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters)
    where TEntity : class, new()
        {
            IQueryable<TEntity> query = GetSourceWithRawSql(querySql, navigationPropertyPath, filter, orderBy, asNoTracking, parameters);
            return query.AsEnumerable();
        }

        /// <summary>
        /// Gets the entities with raw SQL.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return GetEntitiesWithRawSql(querySql, navigationPropertyPath, filter, null, asNoTracking, parameters);
        }

        /// <summary>
        /// Gets the entities with raw SQL.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", bool asNoTracking = false, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return GetEntitiesWithRawSql<TEntity>(querySql, navigationPropertyPath, null, asNoTracking, parameters);
        }

        /// <summary>
        /// Gets the entities with raw SQL.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "", params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return GetEntitiesWithRawSql<TEntity>(querySql, navigationPropertyPath, false, parameters);
        }

        /// <summary>
        /// Gets the entities with raw SQL.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithRawSql<TEntity>(string querySql, string navigationPropertyPath = "")
            where TEntity : class, new()
        {
            return GetEntitiesWithRawSql<TEntity>(querySql, navigationPropertyPath, null);
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IEnumerable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, params SqlParameter[] parameters)
        where TEntity : class, new()
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
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool asNoTracking = false, params SqlParameter[] parameters)
    where TEntity : class, new()
        {
            IQueryable<TEntity> queryEntity = GetEntitiesWithDataReader<TEntity>(querySql, parameters).AsQueryable();

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
        /// GetEntitiesWithDataReader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, string navigationPropertyPath = "", Expression<Func<TEntity, bool>> filter = null, bool asNoTracking = false, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return GetEntitiesWithDataReader(querySql, navigationPropertyPath, filter, null, asNoTracking, parameters);
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="navigationPropertyPath">The navigation property path.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, string navigationPropertyPath = "", bool asNoTracking = false, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return GetEntitiesWithDataReader<TEntity>(querySql, navigationPropertyPath, null, asNoTracking, parameters);
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, bool asNoTracking = false, params SqlParameter[] parameters)
            where TEntity : class, new()
        {
            return GetEntitiesWithDataReader<TEntity>(querySql, string.Empty, asNoTracking, parameters);
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <param name="asNoTracking">if set to <c>true</c> [as no tracking].</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql, bool asNoTracking = false)
          where TEntity : class, new()
        {
            return GetEntitiesWithDataReader<TEntity>(querySql, asNoTracking, null);
        }

        /// <summary>
        /// Gets the entities with data reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="querySql">The query SQL.</param>
        /// <returns>Result.</returns>
        public IQueryable<TEntity> GetEntitiesWithDataReader<TEntity>(string querySql)
      where TEntity : class, new()
        {
            return GetEntitiesWithDataReader<TEntity>(querySql, false);
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
