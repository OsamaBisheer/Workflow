using System.Linq.Expressions;

namespace Workflow.Repository.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string columnName, int direction)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = direction == 1 ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                  new[] { source.ElementType, property.Type },
                                  source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}