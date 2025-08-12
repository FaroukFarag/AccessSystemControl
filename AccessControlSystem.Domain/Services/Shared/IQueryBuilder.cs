using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;

namespace AccessControlSystem.Domain.Services.Shared;

public interface IQueryBuilder<TEntity> where TEntity : class
{
    IQueryable<TEntity> BuildQuery(IBaseSpecification<TEntity>? spec = null);
    IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query, IBaseSpecification<TEntity>? spec);
}
