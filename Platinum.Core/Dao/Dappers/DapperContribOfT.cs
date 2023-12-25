// <copyright file="DapperContrib.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Platinum.Core.Common;
using System.Data;
using System.Data.SqlClient;

namespace Platinum.Core.Dao.Dappers
{
    /// <summary>
    /// DapperQuery.
    /// </summary>
    /// <seealso cref="IDapperContrib" />
    /// <seealso cref="Interfaces.Queries.IDapper" />
    public class DapperContribOfT<TEntity> where TEntity : EntityBase//: IDapperContrib
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperContrib"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DapperContribOfT(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("PlatinumProduct");
        }

        /// <summary>
        /// Deletes the specified object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Result.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(TEntity obj)

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
        public bool Delete(IEnumerable<TEntity> list)

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
        public bool DeleteAll()

        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Result.</returns>
        public TEntity Get(Guid id)

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
        public IEnumerable<TEntity> GetAll()

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
        public long Insert(TEntity obj)

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
        public long Insert(IEnumerable<TEntity> list)

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
        public bool Update(TEntity obj)

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
        public bool Update(IEnumerable<TEntity> list)

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
