// <copyright file="ExpressionHelper.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapVisitor"/> class.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }

    public static class ExpressionHelper
    {
        #region -- Public methods --
        public static Expression<Func<T, bool>> GetCriteriaWhere<T>(Expression<Func<T, object>> e, OperationExpression selectedOperator, object fieldValue)
        {
            string name = GetOperand(e);
            return GetCriteriaWhere<T>(name, selectedOperator, fieldValue);
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T, T2>(Expression<Func<T, object>> e, OperationExpression selectedOperator, object fieldValue)
        {
            string name = GetOperand(e);
            return GetCriteriaWhere<T, T2>(name, selectedOperator, fieldValue);
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T>(string fieldName, OperationExpression selectedOperator, object fieldValue)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = GetProperty(props, fieldName, true);

            var parameter = Expression.Parameter(typeof(T));
            var expressionMember = GetMemberExpression<T>(parameter, fieldName);

            if (prop != null && fieldValue != null)
            {
                BinaryExpression body = null;

                switch (selectedOperator)
                {
                    case OperationExpression.Equals:
                        body = Expression.Equal(expressionMember, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    case OperationExpression.NotEquals:
                        body = Expression.NotEqual(expressionMember, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    case OperationExpression.Minor:
                        body = Expression.LessThan(expressionMember, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    case OperationExpression.MinorEquals:
                        body = Expression.LessThanOrEqual(expressionMember, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    case OperationExpression.Mayor:
                        body = Expression.GreaterThan(expressionMember, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    case OperationExpression.MayorEquals:
                        body = Expression.GreaterThanOrEqual(expressionMember, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    case OperationExpression.Like:
                        MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var bodyLike = Expression.Call(expressionMember, contains, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(bodyLike, parameter);
                    case OperationExpression.StartsWith:
                        MethodInfo startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                        var bodyStartsWith = Expression.Call(expressionMember, startsWith, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(bodyStartsWith, parameter);
                    case OperationExpression.EndsWith:
                        MethodInfo endsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                        var bodyEndsWith = Expression.Call(expressionMember, endsWith, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(bodyEndsWith, parameter);
                    case OperationExpression.Contains:
                        return Contains<T>(fieldValue, parameter, expressionMember);
                    default:
                        throw new Exception("Not implement Operation");
                }
            }
            else
            {
                Expression<Func<T, bool>> filter = x => true;
                return filter;
            }
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T, T2>(string fieldName, OperationExpression selectedOperator, object fieldValue)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = GetProperty(props, fieldName, true);

            var parameter = Expression.Parameter(typeof(T));
            var expressionParameter = GetMemberExpression<T>(parameter, fieldName);

            if (prop != null && fieldValue != null)
            {
                switch (selectedOperator)
                {
                    case OperationExpression.Any:
                        return Any<T, T2>(fieldValue, parameter, expressionParameter);

                    // case OperationExpression.Contains:
                    //    return Contains<T, T2>(fieldValue, parameter, expressionParameter);
                    default:
                        throw new Exception("Not implement Operation");
                }
            }
            else
            {
                Expression<Func<T, bool>> filter = x => true;
                return filter;
            }
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> or)
        {
            if (expr == null)
            {
                return or;
            }

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(new SwapVisitor(expr.Parameters[0], or.Parameters[0]).Visit(expr.Body), or.Body), or.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> and)
        {
            if (expr == null)
            {
                return and;
            }

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(new SwapVisitor(expr.Parameters[0], and.Parameters[0]).Visit(expr.Body), and.Body), and.Parameters);
        }

        #endregion
        #region -- Private methods --

        private static string GetOperand<T>(Expression<Func<T, object>> exp)
        {
            if (!(exp.Body is MemberExpression body))
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            var operand = body.ToString();

            return operand.Substring(2);
        }

        private static MemberExpression GetMemberExpression<T>(ParameterExpression parameter, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                return null;
            }

            var propertiesName = propName.Split('.');
            if (propertiesName.Count() == 2)
            {
                return Expression.Property(Expression.Property(parameter, propertiesName[0]), propertiesName[1]);
            }

            return Expression.Property(parameter, propName);
        }

        private static Expression<Func<T, bool>> Contains<T>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            var list = (List<long>)fieldValue;

            if (list == null || list.Count == 0)
            {
                return x => true;
            }

            MethodInfo containsInList = typeof(List<long>).GetMethod("Contains", new Type[] { typeof(long) });
            var bodyContains = Expression.Call(Expression.Constant(fieldValue), containsInList, memberExpression);

            return Expression.Lambda<Func<T, bool>>(bodyContains, parameterExpression);
        }

        private static Expression<Func<T, bool>> Any<T, T2>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            var lambda = (Expression<Func<T2, bool>>)fieldValue;
            MethodInfo anyMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(T2));

            var body = Expression.Call(anyMethod, memberExpression, lambda);

            return Expression.Lambda<Func<T, bool>>(body, parameterExpression);
        }

        private static PropertyDescriptor GetProperty(PropertyDescriptorCollection props, string fieldName, bool ignoreCase)
        {
            if (!fieldName.Contains('.'))
            {
                return props.Find(fieldName, ignoreCase);
            }

            var fieldNameProperty = fieldName.Split('.');
            return props.Find(fieldNameProperty[0], ignoreCase).GetChildProperties().Find(fieldNameProperty[1], ignoreCase);
        }
        #endregion
    }

    public enum OperationExpression
    {
        Equals,
        NotEquals,
        Minor,
        MinorEquals,
        Mayor,
        MayorEquals,
        Like,
        Contains,
        Any,
        EndsWith,
        StartsWith,
        IsNullOrEmpty,
    }
}
