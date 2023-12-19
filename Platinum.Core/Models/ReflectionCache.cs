using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Platinum.Core.Models
{
    public class ReflectionCache<T> where T : ModelBase
    {
        public Type ModelType { get; private set; }
        public IDictionary<PropertyInfo, Func<T, object>> ModelTypeProperties { get; private set; }
        public DataTable ModelTypeTable { get; private set; }
        public ReflectionCache()
        {
            //Cache type  
            ModelType = typeof(T);

            //Cache PropertyInfo into collection coupled with   
            ModelTypeProperties = ModelType.GetProperties()
                .ToDictionary(k => k, v =>
            {
                var expressionParam = Expression.Parameter(ModelType);
                var getterDelagateExpression = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(
                        Expression.Property(expressionParam, v.Name),
                        typeof(object)
                    ),
                    expressionParam
                ).Compile();
                return getterDelagateExpression;
            });

            //Create structured empty table for the type  
            ModelTypeTable = new DataTable();
            var columns = ModelTypeProperties.Keys.Select
                (
                    p => new DataColumn(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)
                    {
                        AllowDBNull = true
                    }
                ).ToArray();
            ModelTypeTable.Columns.AddRange(columns);
            ModelTypeTable.AcceptChanges();
        }
    }
}