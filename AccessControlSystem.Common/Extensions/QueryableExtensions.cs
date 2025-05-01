using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccessControlSystem.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplyCriteria<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>>? criteria)
        where TEntity : class
    {
        return criteria != null ? query.Where(criteria) : query;
    }

    public static IQueryable<TEntity> ApplyIncludes<TEntity>(
        this IQueryable<TEntity> query,
        List<Expression<Func<TEntity, object>>>? includes)
        where TEntity : class
    {
        if (includes == null)
            return query;

        return includes.Aggregate(query, (current, include) => current.Include(include));
    }

    public static IQueryable<TEntity> ApplyIncludesThen<TEntity>(
        this IQueryable<TEntity> query,
        List<(Expression<Func<TEntity, IEnumerable<object>>> Path, Expression<Func<object, object>> ThenInclude)>? includesThen)
        where TEntity : class
    {
        if (includesThen == null || includesThen.Count == 0)
            return query;

        foreach (var (path, thenInclude) in includesThen)
        {
            query = query.Include(path).ThenInclude(thenInclude);
        }

        return query;
    }

    public static IQueryable<TEntity> ApplyOrderBy<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>? orderBy)
        where TEntity : class
    {
        return orderBy != null ? query.OrderBy(orderBy) : query;
    }

    public static IQueryable<TEntity> ApplyOrderByDescending<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>? orderByDescending)
        where TEntity : class
    {
        return orderByDescending != null ? query.OrderByDescending(orderByDescending) : query;
    }
}