using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;

namespace AccessControlSystem.Infrastructure.Data.Shared.Helpers;

public class QueryBuilder<TEntity> : IQueryBuilder<TEntity> where TEntity : class
{
    private readonly AccessControlDbContext _context;

    public QueryBuilder(AccessControlDbContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> BuildQuery(IBaseSpecification<TEntity>? spec = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        return ApplySpecification(query, spec);
    }

    public IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query, IBaseSpecification<TEntity>? spec)
    {
        if (spec == null)
            return query;

        return query
            .ApplyCriteria(spec.Criteria)
            .ApplyIncludes(spec.Includes)
            .ApplyIncludeChains(spec.IncludeChains)
            .ApplyOrderBy(spec.OrderBy)
            .ApplyOrderByDescending(spec.OrderByDescending);
    }
}

