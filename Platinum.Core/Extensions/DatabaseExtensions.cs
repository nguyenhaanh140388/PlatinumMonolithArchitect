// <copyright file="DatabaseExtensions.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Platinum.Core.Extensions;

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// DatabaseExtensions.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Executes the SQL query.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Result.</returns>
        public static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                return rawSqlCommand
                    .RelationalCommand
                     .ExecuteReader(new RelationalCommandParameterObject());
                //.ExecuteReader(
                //    databaseFacade.GetService<IRelationalConnection>(),
                //    parameterValues: rawSqlCommand.ParameterValues);
            }
        }

        /// <summary>
        /// Executes the SQL query asynchronous.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<RelationalDataReader> ExecuteSqlQueryAsync(
            this DatabaseFacade databaseFacade,
                                                             string sql,
                                                             CancellationToken cancellationToken = default,
                                                             params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                return await rawSqlCommand
                    .RelationalCommand
                    .ExecuteReaderAsync(new RelationalCommandParameterObject());
                //.ExecuteReaderAsync(
                //    databaseFacade.GetService<IRelationalConnection>(),
                //    parameterValues: rawSqlCommand.ParameterValues,
                //    cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        /// Set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbContext">DbContext.</param>
        /// <param name="t">type of generic.</param>
        /// <returns>
        /// IQueryable of type.
        /// </returns>
        public static IQueryable<TEntity> Set<TEntity>(this DbContext dbContext, Type t)
        {
            return (IQueryable<TEntity>)dbContext.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(dbContext, null);
        }

        /// <summary>
        /// Set the specified value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="value">The value.</param>
        /// <returns>Result.</returns>
        private static IQueryable<TEntity> Set<TEntity>(this IQueryable<TEntity> entities, string value)
        {
            foreach (var prop in entities.ElementType.GetProperties())
            {
                var prop1 = prop;
                entities = entities.Where(l => Equals(prop1.GetValue(l, null), value));
            }

            return entities;
        }

        /// <summary>
        /// Gets the SQL parameters.
        /// </summary>
        /// <param name="listParams">The list parameters.</param>
        /// <param name="matchedParameterNames">The matched parameter names.</param>
        /// <returns>Result.</returns>
        public static List<SqlParameter> GetSqlParameters(this SqlParameter[] listParams, MatchCollection matchedParameterNames)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (Match match in matchedParameterNames)
            {
                foreach (object parameter in listParams)
                {
                    switch (Type.GetTypeCode(parameter.GetType()))
                    {
                        case TypeCode.Int32:
                            sqlParameters.Add(new SqlParameter()
                            {
                                DbType = System.Data.DbType.Int32,
                                ParameterName = match.Value,
                                Value = parameter,
                            });
                            break;
                        case TypeCode.String:
                            sqlParameters.Add(new SqlParameter()
                            {
                                DbType = System.Data.DbType.String,
                                ParameterName = match.Value,
                                Value = parameter,
                            });
                            break;
                        case TypeCode.Boolean:
                            sqlParameters.Add(new SqlParameter()
                            {
                                DbType = System.Data.DbType.Boolean,
                                ParameterName = match.Value,
                                Value = parameter,
                            });
                            break;
                        case TypeCode.DateTime:
                            sqlParameters.Add(new SqlParameter()
                            {
                                DbType = System.Data.DbType.DateTime,
                                ParameterName = match.Value,
                                Value = parameter,
                            });
                            break;
                    }

                    break;
                }
            }

            return sqlParameters;
        }

        /// <summary>
        /// Includes the specified navigation property paths.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="navigationPropertyPaths">The navigation property paths.</param>
        /// <returns>Result.</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }

        /// <summary>
        /// Gets the include paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clrEntityType">Type of the color entity.</param>
        /// <param name="maxDepth">The maximum depth.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxDepth.</exception>
        public static IEnumerable<string> GetIncludePaths(this DbContext context, Type clrEntityType, int maxDepth = int.MaxValue)
        {
            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDepth));
            }

            var entityType = context.Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();
            while (true)
            {
                var entityNavigations = new List<INavigation>();
                if (stack.Count <= maxDepth)
                {
                    foreach (var navigation in entityType.GetNavigations())
                    {
                        if (includedNavigations.Add(navigation))
                        {
                            entityNavigations.Add(navigation);
                        }
                    }
                }

                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                    {
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                    }
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null)
                        {
                            includedNavigations.Add(inverseNavigation);
                        }
                    }

                    stack.Push(entityNavigations.GetEnumerator());
                }

                while (stack.Count > 0 && !stack.Peek().MoveNext())
                {
                    stack.Pop();
                }

                if (stack.Count == 0)
                {
                    break;
                }

                entityType = stack.Peek().Current.TargetEntityType;
            }
        }

        /// <summary>
        /// IncludeAll.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="queryable">IQueryable.</param>
        /// <returns>
        /// IQueryable of TEntity.
        /// </returns>
        public static IQueryable<TEntity> IncludeAll<TEntity>(this IQueryable<TEntity> queryable)
            where TEntity : class
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.Name.Contains("ICollection"))
                {
                    queryable = queryable.Include(property.Name);
                }
            }

            return queryable;
        }
    }
}
