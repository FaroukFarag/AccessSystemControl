using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Shared.Filters;
using AccessControlSystem.Infrastructure.Data.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

public class BaseRepository<TEntity, TPrimaryKey>(
    AccessControlDbContext context,
    ISpecificationCombiner<TEntity> specificationCombiner)
    : IBaseRepository<TEntity, TPrimaryKey>
    where TEntity : class
{
    private readonly AccessControlDbContext _context = context;
    private readonly ISpecificationCombiner<TEntity> _specificationCombiner = specificationCombiner;

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);

        return entity;
    }

    public TEntity Update(TEntity newEntity)
    {
        _context.Set<TEntity>().Update(newEntity);

        return newEntity;
    }

    public TEntity Delete(TPrimaryKey id)
    {
        var entity = _context.Set<TEntity>().Find(id) ??
            throw new ArgumentException($"Entity with id {id} not found.");

        _context.Set<TEntity>().Remove(entity);

        return entity;
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public async Task<TEntity> GetAsync(TPrimaryKey id, IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        if (id is ITuple)
        {
            var predicate = CompositeKeyHelper.BuildCompositeKeyPredicate<TEntity, TPrimaryKey>(id);
            return await query.FirstOrDefaultAsync(predicate)
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }

        else
        {
            return await query.FirstOrDefaultAsync(e => EF.Property<TPrimaryKey>(e, "Id")!.Equals(id))
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllPaginatedAsync(
        PaginatedModel paginatedModel,
        IBaseSpecification<TEntity>? spec = null)
    {
        paginatedModel.PageNumber = paginatedModel.PageNumber <= 0 ? 1 : paginatedModel.PageNumber;
        var query = ApplySpecification(spec);

        return await query
            .Skip((paginatedModel.PageNumber - 1) * paginatedModel.PageSize)
            .Take(paginatedModel.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllFilteredAsync<TFilterDto>(
        TFilterDto filterDto,
        IBaseSpecification<TEntity>? spec = null)
    {
        if (filterDto == null)
            throw new ArgumentNullException(nameof(filterDto));

        var predicate = filterDto.ToPredicate<TEntity, TFilterDto>();
        var combinedSpec = _specificationCombiner.Combine(spec, predicate);
        var query = ApplySpecification(combinedSpec);

        return await query.ToListAsync();
    }

    public async Task<long> GetCountAsync(IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.CountAsync();
    }

    public async Task<decimal> GetSumAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.SumAsync(selector);
    }

    public async Task<decimal> GetAverageAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.AverageAsync(selector);
    }

    public async Task<TResult> GetMaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.MaxAsync(selector);
    }

    public async Task<TResult> GetMinAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.MinAsync(selector);
    }

    private IQueryable<TEntity> ApplySpecification(IBaseSpecification<TEntity>? spec)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (spec == null)
            return query;

        return query
            .ApplyCriteria(spec.Criteria)
            .ApplyIncludes(spec.Includes)
            .ApplyOrderBy(spec.OrderBy)
            .ApplyOrderByDescending(spec.OrderByDescending);
    }
}
