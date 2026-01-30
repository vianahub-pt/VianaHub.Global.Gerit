using System.Linq.Expressions;
using System.Reflection;

namespace VianaHub.Global.Gerit.Domain.Tools.Pagination;

public static class CreateSort
{
    /// <summary>
    /// Cria uma expressão de ordenação baseada no nome da propriedade
    /// Esta implementação tenta localizar a propriedade/field de forma case-insensitive e
    /// cai para um fallback seguro (propriedade 'Id' quando disponível) ou uma constante,
    /// evitando lançar exceções quando o campo informado não existir.
    /// </summary>
    public static Expression<Func<TEntity, object>> SortBy<TEntity>(string orderBy)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");

        if (string.IsNullOrWhiteSpace(orderBy))
        {
            // Retorna expressão constante segura quando não há ordenação
            var constExpr = Expression.Convert(Expression.Constant(0), typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(constExpr, parameter);
        }

        Expression propertyExpression = parameter;
        var currentType = typeof(TEntity);

        // Helper local to build a safe fallback (Id or constant)
        static Expression<Func<TEntity, object>> SafeFallback<TEntity>(ParameterExpression param)
        {
            var type = typeof(TEntity);
            var idProp = type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (idProp != null)
            {
                var prop = Expression.Property(param, idProp);
                var conv = Expression.Convert(prop, typeof(object));
                return Expression.Lambda<Func<TEntity, object>>(conv, param);
            }

            var constExpr = Expression.Convert(Expression.Constant(0), typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(constExpr, param);
        }

        try
        {
            if (orderBy.Contains('.'))
            {
                foreach (var item in orderBy.Split('.', StringSplitOptions.RemoveEmptyEntries))
                {
                    // procura PropertyInfo (case-insensitive)
                    var propInfo = currentType.GetProperty(item, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (propInfo != null)
                    {
                        propertyExpression = Expression.Property(propertyExpression, propInfo);
                        currentType = propInfo.PropertyType;
                        continue;
                    }

                    // procura field
                    var fieldInfo = currentType.GetField(item, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (fieldInfo != null)
                    {
                        propertyExpression = Expression.Field(propertyExpression, fieldInfo);
                        currentType = fieldInfo.FieldType;
                        continue;
                    }

                    // Não encontrou a parte do caminho -> fallback seguro
                    return SafeFallback<TEntity>(parameter);
                }
            }
            else
            {
                // procura PropertyInfo (case-insensitive)
                var propInfo = currentType.GetProperty(orderBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propInfo != null)
                {
                    propertyExpression = Expression.Property(parameter, propInfo);
                }
                else
                {
                    var fieldInfo = currentType.GetField(orderBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (fieldInfo != null)
                    {
                        propertyExpression = Expression.Field(parameter, fieldInfo);
                    }
                    else
                    {
                        // tenta fallback para 'Id' ou expressão constante
                        return SafeFallback<TEntity>(parameter);
                    }
                }
            }

            var conversion = Expression.Convert(propertyExpression, typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
        }
        catch
        {
            // Em qualquer falha inesperada, usar fallback seguro para evitar lançar exceções
            return SafeFallback<TEntity>(parameter);
        }
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
        var orderExpression = SortBy<TEntity>(order?.SortBy ?? string.Empty);
        var orderType = order?.SortDirection?.ToLower() ?? "asc";

        return orderType == "desc"
            ? query.OrderByDescending(orderExpression)
            : query.OrderBy(orderExpression);
    }
}
