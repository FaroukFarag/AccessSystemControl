using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Shared;
using System.Linq.Expressions;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;

public interface IReadRepository<TEntity, TPrimaryKey> where TEntity : class
{
    Task<TEntity> GetAsync(
        TPrimaryKey id,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
        where TResult : class;

    Task<IEnumerable<TEntity>> GetAllPaginatedAsync(
        PaginatedModel paginatedModel,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TResult>> GetAllPaginatedAsync<TResult>(
        PaginatedModel paginatedModel,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
        where TResult : class;

    Task<IEnumerable<TEntity>> GetAllFilteredAsync<TFilterDto>(
        TFilterDto filterDto,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TResult>> GetAllFilteredAsync<TFilterDto, TResult>(
        TFilterDto filterDto,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
        where TResult : class;
}
