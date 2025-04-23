using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Shared;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;

public interface IReadRepository<TEntity, TPrimaryKey> where TEntity : class
{
    Task<TEntity> GetAsync(
        TPrimaryKey id,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TEntity>> GetAllPaginatedAsync(
        PaginatedModel paginatedModel,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TEntity>> GetAllFilteredAsync<TFilterDto>(
        TFilterDto filterDto,
        IBaseSpecification<TEntity>? spec = null);
}
