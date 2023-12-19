// <copyright file="EntityQueryBase.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Platinum.Infrastructure.Data.Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Platinum.Infrastructure.Repositories
{
    /// <summary>
    /// EntityQueryBase.
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly IDapperConnectionFactory factory;

        protected BaseRepository(IDapperConnectionFactory factory)
        {
            this.factory = factory;
        }

        // use for buffered queries that return a type
        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = await factory.CreateSqlConnection())
                {
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        protected async Task<T> WithTransaction<T>(Func<IDbConnection, Task<T>> getData, DbTransaction dbTransaction = null)
        {
            try
            {
                using (var connection = await factory.CreateSqlConnection())
                {
                    using (dbTransaction ?? connection.BeginTransaction())
                    {
                        var result = await getData(connection);
                        dbTransaction.Commit();
                        return result;
                    }
                }
            }
            catch (TimeoutException ex)
            {
                dbTransaction.Rollback();
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                dbTransaction.Rollback();
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        // use for buffered queries that do not return a type
        protected async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            try
            {
                using (var connection = await factory.CreateSqlConnection())
                {
                    await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        // use for non-buffered queries that return a type
        protected async Task<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            try
            {
                using (var connection = await factory.CreateSqlConnection())
                {
                    var data = await getData(connection);
                    return await process(data);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

    }
}
