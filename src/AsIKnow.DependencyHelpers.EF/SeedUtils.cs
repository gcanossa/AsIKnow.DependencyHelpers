using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AsIKnow.DependencyHelpers.EF
{
    public static class SeedUtils
    {
        public static Expression<Func<T, bool>> GetCompareExpression<T>(T obj, Expression<Func<T, object>> excludedProperties = null, bool excludeCollections = true)
        {
            List<string> excludedProps = new List<string>();
            if (excludedProperties != null)
            {
                if (excludedProperties.Body is NewExpression)
                {
                    excludedProps.AddRange(((NewExpression)excludedProperties.Body).Type.GetProperties().Select(p => p.Name));
                }
                else if (excludedProperties.Body is MemberExpression)
                {
                    excludedProps.Add(((MemberExpression)excludedProperties.Body).Member.Name);
                }
                else if (excludedProperties.Body is UnaryExpression)
                {
                    excludedProps.Add(((MemberExpression)((UnaryExpression)excludedProperties.Body).Operand).Member.Name);
                }
            }

            if (excludeCollections)
            {
                excludedProps.AddRange(typeof(T).GetProperties().Where(p => typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType)).Select(p => p.Name));
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "p");
            Expression predicateBody = null;

            foreach (PropertyInfo pinfo in typeof(T).GetProperties().Where(p => !excludedProps.Contains(p.Name)))
            {
                Expression left = Expression.Property(pe, typeof(T).GetProperty(pinfo.Name));
                Expression right = Expression.Constant(pinfo.GetValue(obj), pinfo.PropertyType);
                Expression e2 = Expression.Equal(left, right);

                if (predicateBody == null)
                    predicateBody = e2;
                else
                    predicateBody = Expression.AndAlso(predicateBody, e2);
            }

            return Expression.Lambda<Func<T, bool>>(predicateBody, new ParameterExpression[] { pe });
        }

        public static D AddIfNotExists<T,D>(this D ctx, T obj, Expression<Func<T, object>> excludeFromCompare = null) where T : class where D : DbContext
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));

            if (ctx.Set<T>().Count(GetCompareExpression<T>(obj, excludeFromCompare)) == 0)
                ctx.Set<T>().Add(obj);

            return ctx;
        }
    }
}
