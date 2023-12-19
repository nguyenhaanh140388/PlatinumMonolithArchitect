using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Platinum.Core.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Platinum.Core.Extensions
{
    public static class IQueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly FieldInfo queryContextFactoryField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryContextFactory");
        private static readonly FieldInfo loggerField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_logger");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        private static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set));
        private static readonly MethodInfo _asQueryableMethodInfo = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == nameof(Queryable.AsQueryable)
            && m.IsGenericMethod);

        //public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        //{
        //    var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
        //    var queryModelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
        //    var queryModel = queryModelGenerator.ParseQuery(query.Expression);
        //    var database = DataBaseField.GetValue(queryCompiler);
        //    var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
        //    var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
        //    var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
        //    modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
        //    var sql = modelVisitor.Queries.First().ToString();

        //    return sql;
        //}

        //public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlFull<TEntity>(IQueryable<TEntity> query) where TEntity : class
        //{
        //    var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
        //    var queryContextFactory = (IQueryContextFactory)queryContextFactoryField.GetValue(queryCompiler);
        //    var logger = (Microsoft.EntityFrameworkCore.Diagnostics.IDiagnosticsLogger<DbLoggerCategory.Query>)loggerField.GetValue(queryCompiler);
        //    var queryContext = queryContextFactory.Create();
        //    var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
        //    var newQueryExpression = modelGenerator.ExtractParameters(logger, query.Expression, queryContext);
        //    var queryModel = modelGenerator.ParseQuery(newQueryExpression);
        //    var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
        //    var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
        //    var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
        //    var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();

        //    modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
        //    var command = modelVisitor.Queries.First().CreateDefaultQuerySqlGenerator()
        //        .GenerateSql(queryContext.ParameterValues);

        //    return (command.CommandText, queryContext.ParameterValues);
        //}


        public static IQueryable Query(this DbContext context, string entityName) =>
           context.Query(context.Model.FindEntityType(entityName).ClrType);

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
        public static IQueryable<TEntity> AsSubQuery<TEntity>(
                      this IQueryable<TEntity> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!(source.Provider is EntityQueryProvider))
                return source;

            var methodCall = Expression.Call(
                                    null,
                                    _asQueryableMethodInfo.MakeGenericMethod(typeof(TEntity)),
                                    source.Expression);

            return source.Provider.CreateQuery<TEntity>(methodCall);
        }

        public static async Task<PaginatedList<T>> ToPaginatedList<T>(
            this IQueryable<T> query,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = query.Count();
            var collection = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }
    }
}
