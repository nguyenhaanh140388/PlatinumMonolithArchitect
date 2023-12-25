// <copyright file="DapperQuery.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Dapper;
using Dapper;
using DapperExtensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace Platinum.Core.Dao.Dappers
{
    /// <summary>
    /// DapperQuery.
    /// </summary>
    /// <seealso cref="Infrastructure.Abstractions.Dapper.IDapper" />
    public class DapperQuery : IDapper
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperQuery" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DapperQuery(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection => new SqlConnection(connectionString);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DbConnection.Dispose();
        }

        /// <summary>
        /// Executes the specified sp.
        /// </summary>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        /// Result.
        /// </returns>
        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return DbConnection.Execute(sp, parms, commandType: commandType);
        }

        /// <summary>
        /// Executes the specified sp.
        /// </summary>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Result.</returns>
        public int Execute(string sp, List<DynamicParameters> parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return DbConnection.Execute(sp, parms, commandType: commandType);
        }

        /// <summary>
        /// Gets the specified sp.
        /// </summary>
        /// <typeparam name="TEntity">Entity.</typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        /// Result.
        /// </returns>
        public TEntity GetEntity<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return DbConnection.Query<TEntity>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TEntity">Entity.</typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        /// Result.
        /// </returns>
        public List<TEntity> GetEntities<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return DbConnection.Query<TEntity>(sp, parms, commandType: commandType).ToList();
        }

        /// <summary>
        /// Gets the multi entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        public void GetMultiEntity<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            GridReader gridReader = DbConnection.QueryMultiple(sp, parms, null, 3600, commandType);
        }

        /// <summary>
        /// Gets the dbconnection.
        /// </summary>
        /// <returns>
        /// Result.
        /// </returns>
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Inserts the specified sp.
        /// </summary>
        /// <typeparam name="TEntity">Entity.</typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        /// Result.
        /// </returns>
        public TEntity Insert<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            TEntity result;

            try
            {
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }

                using (IDbTransaction tran = DbConnection.BeginTransaction())
                {
                    try
                    {
                        result = DbConnection.Query<TEntity>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (DbConnection.State == ConnectionState.Open)
                {
                    DbConnection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the specified sp.
        /// </summary>
        /// <typeparam name="TEntity">Entity.</typeparam>
        /// <param name="sp">The sp.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        /// TEntity.
        /// </returns>
        public TEntity Update<TEntity>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            TEntity result;

            try
            {
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }

                using (IDbTransaction tran = DbConnection.BeginTransaction())
                {
                    try
                    {
                        result = DbConnection.Query<TEntity>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (DbConnection.State == ConnectionState.Open)
                {
                    DbConnection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dapperQueryParameters">The dapper query parameters.</param>
        /// <returns>
        /// IEnumerable.
        /// </returns>
        public IEnumerable<TEntity> GetEntities<TEntity>(IDapperQueryParameters<TEntity> dapperQueryParameters)
            where TEntity : class, new()
        {
            List<ISort> sortList = new List<ISort>();

            foreach (var sortTuple in dapperQueryParameters.SortExpressions)
            {
                sortList.Add(Predicates.Sort(sortTuple.Item1.AsExpression(), sortTuple.Item2));
            }

            PredicateGroup rootPredicate = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

            foreach (var operatorTuple in dapperQueryParameters.Operators)
            {
                var predicateGroup = new PredicateGroup { Operator = operatorTuple.Item1, Predicates = new List<IPredicate>() };

                foreach (var predicatesTuple in dapperQueryParameters.PredicatesExpressions)
                {
                    if (predicatesTuple.Item4 == operatorTuple.Item2)
                    {
                        predicateGroup.Predicates.Add(Predicates.Field(predicatesTuple.Item1.AsExpression(), predicatesTuple.Item2, predicatesTuple.Item3));
                    }
                }

                rootPredicate.Predicates.Add(predicateGroup);
            }

            return DbConnection.GetList<TEntity>(rootPredicate, sortList, dapperQueryParameters.Transaction);
        }
    }
}
