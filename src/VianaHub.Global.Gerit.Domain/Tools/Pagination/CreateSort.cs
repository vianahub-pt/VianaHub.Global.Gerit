using System.Linq.Expressions;

namespace VianaHub.Global.Gerit.Domain.Tools.Pagination;

public static class CreateSort
{
    /// <summary>
    /// Cria uma expressão de ordenação baseada no nome da propriedade
    /// </summary>
    public static Expression<Func<TEntity, object>> SortBy<TEntity>(string orderBy)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression property = parameter;

        if (orderBy.Contains('.'))
        {
            foreach (var item in orderBy.Split('.'))
            {
                property = Expression.PropertyOrField(property, item);
            }
        }
        else
        {
            property = Expression.Property(parameter, orderBy);
        }

        var conversion = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
        return lambda;
    }

    /// <summary>
    /// Aplica ordenação na query baseado nos parâmetros do Order (SortBy e SortDirection)
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade</typeparam>
    /// <param name="query">Query a ser ordenada</param>
    /// <param name="order">Objeto contendo SortBy e SortDirection</param>
    /// <returns>Query ordenada</returns>
    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(
        IQueryable<TEntity> query,
        Order order)
    {
        var orderExpression = SortBy<TEntity>(order.SortBy);
        var orderType = order.SortDirection?.ToLower() ?? "asc";

        return orderType == "desc"
            ? query.OrderByDescending(orderExpression)
            : query.OrderBy(orderExpression);
    }
}
