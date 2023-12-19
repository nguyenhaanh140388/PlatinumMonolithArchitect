// <copyright file="DapperContrib.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Platinum.Infrastructure.Dao.Dappers
{
    /// <summary>
    /// DapperQuery.
    /// </summary>
    /// <seealso cref="IDapperContrib" />
    /// <seealso cref="Interfaces.Queries.IDapper" />
    public class DapperContrib : IDapperContrib
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperContrib"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DapperContrib(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Anhny010920Catalog");
        }

        /// <summary>
        /// Deletes the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete<TEntity>(TEntity obj)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Delete(obj);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Deletes the specified list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete<TEntity>(IEnumerable<TEntity> list)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Delete(list);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteAll<TEntity>()
            where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Result.</returns>
        public TEntity Get<TEntity>(Guid id)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Get<TEntity>(id);
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// IEnumerable of TEntity.
        /// </returns>
        public IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.GetAll<TEntity>();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public long Insert<TEntity>(TEntity obj)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Insert(obj);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserts the specified list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public long Insert<TEntity>(IEnumerable<TEntity> list)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Insert(list);
                }
                catch (Exception)
                {
                    return 0;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Updates the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Update<TEntity>(TEntity obj)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Update(obj);
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Updates the specified list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Update<TEntity>(IEnumerable<TEntity> list)
            where TEntity : class, new()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    return db.Update(list);
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
    }
}
