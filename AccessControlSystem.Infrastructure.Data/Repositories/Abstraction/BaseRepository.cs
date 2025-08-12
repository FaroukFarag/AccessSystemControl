// 1. Separate Query Building from Repository
using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Shared.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

public class BaseRepository<TEntity, TPrimaryKey>(
    AccessControlDbContext context,
    ISpecificationCombiner<TEntity> specificationCombiner,
    IQueryBuilder<TEntity> queryBuilder,
    IEntityFinder<TEntity, TPrimaryKey> entityFinder,
    IPaginationService paginationService,
    ILogger<BaseRepository<TEntity, TPrimaryKey>> logger) : IBaseRepository<TEntity, TPrimaryKey>
    where TEntity : class
{
    private readonly AccessControlDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ISpecificationCombiner<TEntity> _specificationCombiner = specificationCombiner ?? throw new ArgumentNullException(nameof(specificationCombiner));
    private readonly IQueryBuilder<TEntity> _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
    private readonly IEntityFinder<TEntity, TPrimaryKey> _entityFinder = entityFinder ?? throw new ArgumentNullException(nameof(entityFinder));
    private readonly IPaginationService _paginationService = paginationService ?? throw new ArgumentNullException(nameof(paginationService));
    private readonly ILogger<BaseRepository<TEntity, TPrimaryKey>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await _context.Set<TEntity>().AddAsync(entity);

            _logger.LogDebug("Entity {EntityType} added to context", typeof(TEntity).Name);

            return entity;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var entityList = entities.ToList();
        if (!entityList.Any())
            return;

        try
        {
            await _context.Set<TEntity>().AddRangeAsync(entityList);
            _logger.LogDebug("{Count} entities of type {EntityType} added to context",
                entityList.Count, typeof(TEntity).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating {Count} entities of type {EntityType}",
                entityList.Count, typeof(TEntity).Name);
            throw;
        }
    }

    public virtual async Task<TEntity> GetAsync(TPrimaryKey id,
        IBaseSpecification<TEntity>? spec = null)
    {
        try
        {
            var query = _queryBuilder.BuildQuery(spec);
            var entity = await _entityFinder.FindByIdAsync(id, query);

            if (entity == null)
            {
                _logger.LogWarning("Entity {EntityType} with id {Id} not found", typeof(TEntity).Name, id);

                throw new ArgumentException($"Entity with id {id} not found.");
            }

            return entity;
        }

        catch (Exception ex) when (ex is not ArgumentException)
        {
            _logger.LogError(ex, "Error getting entity {EntityType} with id {Id}", typeof(TEntity).Name, id);

            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecification<TEntity>? spec = null)
    {
        try
        {
            var query = _queryBuilder.BuildQuery(spec);

            return await query.AsNoTracking().ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all entities of type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<IEnumerable<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
        where TResult : class
    {
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var query = _queryBuilder.BuildQuery(spec).AsNoTracking();

            return await query.Select(selector).ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projected entities of type {EntityType} to {ResultType}",
                typeof(TEntity).Name, typeof(TResult).Name);

            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllPaginatedAsync(
        PaginatedModel paginatedModel,
        IBaseSpecification<TEntity>? spec = null)
    {
        ArgumentNullException.ThrowIfNull(paginatedModel);

        try
        {
            var query = _queryBuilder.BuildQuery(spec);
            var paginatedQuery = _paginationService.ApplyPagination(query, paginatedModel);

            return await paginatedQuery.AsNoTracking().ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated entities of type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<IEnumerable<TResult>> GetAllPaginatedAsync<TResult>(
        PaginatedModel paginatedModel,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
        where TResult : class
    {
        ArgumentNullException.ThrowIfNull(paginatedModel);
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var query = _queryBuilder.BuildQuery(spec).AsNoTracking();
            var paginatedQuery = _paginationService.ApplyPagination(query, paginatedModel);

            return await paginatedQuery.Select(selector).ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated projected entities of type {EntityType} to {ResultType}",
                typeof(TEntity).Name, typeof(TResult).Name);

            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllFilteredAsync<TFilterDto>(
        TFilterDto filterDto,
        IBaseSpecification<TEntity>? spec = null)
    {
        ArgumentNullException.ThrowIfNull(filterDto);

        try
        {
            var predicate = filterDto.ToPredicate<TEntity, TFilterDto>();
            var combinedSpec = _specificationCombiner.Combine(spec, predicate);
            var query = _queryBuilder.BuildQuery(combinedSpec);

            return await query.AsNoTracking().ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting filtered entities of type {EntityType} with filter {FilterType}",
                typeof(TEntity).Name, typeof(TFilterDto).Name);

            throw;
        }
    }

    public virtual async Task<IEnumerable<TResult>> GetAllFilteredAsync<TFilterDto,
        TResult>(TFilterDto filterDto,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
        where TResult : class
    {
        ArgumentNullException.ThrowIfNull(filterDto);
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var predicate = filterDto.ToPredicate<TEntity, TFilterDto>();
            var combinedSpec = _specificationCombiner.Combine(spec, predicate);
            var query = _queryBuilder.BuildQuery(combinedSpec).AsNoTracking();

            return await query.Select(selector).ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting filtered projected entities of type {EntityType} to {ResultType} with filter {FilterType}",
                typeof(TEntity).Name, typeof(TResult).Name, typeof(TFilterDto).Name);

            throw;
        }
    }

    public virtual TEntity Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            _context.Set<TEntity>().Update(entity);
            _logger.LogDebug("Entity {EntityType} marked for update", typeof(TEntity).Name);

            return entity;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var entityList = entities.ToList();

        if (entityList.Count == 0)
            return;

        try
        {
            _context.Set<TEntity>().UpdateRange(entityList);
            _logger.LogDebug("{Count} entities of type {EntityType} marked for update",
                entityList.Count, typeof(TEntity).Name);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating {Count} entities of type {EntityType}",
                entityList.Count, typeof(TEntity).Name);

            throw;
        }
    }

    public virtual TEntity Delete(TPrimaryKey id)
    {
        try
        {
            var entity = _context.Set<TEntity>().Find(id);

            if (entity == null)
            {
                _logger.LogWarning("Entity {EntityType} with id {Id} not found for deletion", typeof(TEntity).Name, id);

                throw new ArgumentException($"Entity with id {id} not found.");
            }

            _context.Set<TEntity>().Remove(entity);
            _logger.LogDebug("Entity {EntityType} with id {Id} marked for deletion", typeof(TEntity).Name, id);

            return entity;
        }

        catch (Exception ex) when (ex is not ArgumentException)
        {
            _logger.LogError(ex, "Error deleting entity {EntityType} with id {Id}", typeof(TEntity).Name, id);

            throw;
        }
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var entityList = entities.ToList();

        if (entityList.Count == 0)
            return;

        try
        {
            _context.Set<TEntity>().RemoveRange(entityList);
            _logger.LogDebug("{Count} entities of type {EntityType} marked for deletion",
                entityList.Count, typeof(TEntity).Name);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting {Count} entities of type {EntityType}",
                entityList.Count, typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<long> GetCountAsync(IBaseSpecification<TEntity>? spec = null)
    {
        try
        {
            var query = _queryBuilder.BuildQuery(spec);

            return await query.CountAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting count for entity type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<bool> ExistsAsync(TPrimaryKey id, IBaseSpecification<TEntity>? spec = null)
    {
        try
        {
            var query = _queryBuilder.BuildQuery(spec);
            var predicate = _entityFinder.BuildIdPredicate(id);
            return await query.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence for entity {EntityType} with id {Id}", typeof(TEntity).Name, id);
            throw;
        }
    }

    public virtual async Task<decimal> GetSumAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var query = _queryBuilder.BuildQuery(spec);

            return await query.SumAsync(selector);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating sum for entity type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<decimal> GetAverageAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var query = _queryBuilder.BuildQuery(spec);

            return await query.AverageAsync(selector);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating average for entity type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<TResult> GetMaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var query = _queryBuilder.BuildQuery(spec);

            return await query.MaxAsync(selector);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating max for entity type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }

    public virtual async Task<TResult> GetMinAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        ArgumentNullException.ThrowIfNull(selector);

        try
        {
            var query = _queryBuilder.BuildQuery(spec);

            return await query.MinAsync(selector);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating min for entity type {EntityType}", typeof(TEntity).Name);

            throw;
        }
    }
}
