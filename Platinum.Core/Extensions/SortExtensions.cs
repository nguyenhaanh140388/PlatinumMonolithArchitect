using Anhny010920.Core.Models;
using Platinum.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Platinum.Core.Extensions
{
    public static class SortExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByStrValues) where TEntity : class
        {
            var queryExpr = source.Expression;
            var methodAsc = "OrderBy";
            var methodDesc = "OrderByDescending";



            var orderByValues = orderByStrValues.Trim().Split(',').Select(x => x.Trim()).ToList();



            foreach (var orderPairCommand in orderByValues)
            {
                var command = orderPairCommand.ToUpper().EndsWith("DESC") ? methodDesc : methodAsc;



                //Get propertyname and remove optional ASC or DESC
                var propertyName = orderPairCommand.Split(' ')[0].Trim();



                var type = typeof(TEntity);
                var parameter = Expression.Parameter(type, "p");



                PropertyInfo property;
                MemberExpression propertyAccess;



                if (propertyName.Contains('.'))
                {
                    // support to be sorted on child fields.
                    var childProperties = propertyName.Split('.');



                    property = SearchProperty(typeof(TEntity), childProperties[0]);
                    if (property == null)
                        continue;



                    propertyAccess = Expression.MakeMemberAccess(parameter, property);



                    for (int i = 1; i < childProperties.Length; i++)
                    {
                        var t = property.PropertyType;
                        property = SearchProperty(t, childProperties[i]);



                        if (property == null)
                            continue;



                        propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    }
                }
                else
                {
                    property = null;
                    property = SearchProperty(type, propertyName);



                    if (property == null)
                        continue;



                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                }



                var orderByExpression = Expression.Lambda(propertyAccess, parameter);



                queryExpr = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, queryExpr, Expression.Quote(orderByExpression));



                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }



            return source.Provider.CreateQuery<TEntity>(queryExpr); ;
        }

        public static string GetSortString(this List<Sort> sorts)
        {
            var multiSortByPriority = sorts.OrderBy(x => x.Priority).ToList();
            var strOrder = string.Join(",", multiSortByPriority.Select
            (
            x => new
            {
                a = string.Format("{0} {1}", x.Prop, x.IsDesc ? "DESC" : "ASC")
            }).Select(x => x.a));



            return strOrder;
        }

        private static PropertyInfo SearchProperty(Type type, string propertyName)
        {
            foreach (var item in type.GetProperties())
                if (item.Name.ToLower() == propertyName.ToLower())
                    return item;
            return null;
        }
    }
}
