using System.Linq.Expressions;

namespace AccessControlSystem.Domain.Services.Shared;

public interface IEntityFinder<TEntity, TPrimaryKey> where TEntity : class
{
    Task<TEntity?> FindByIdAsync(TPrimaryKey id, IQueryable<TEntity> query);
    Expression<Func<TEntity, bool>> BuildIdPredicate(TPrimaryKey id);
}
