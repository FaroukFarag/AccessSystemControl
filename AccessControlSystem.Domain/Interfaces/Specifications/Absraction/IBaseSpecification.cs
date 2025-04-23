using System.Linq.Expressions;

namespace AccessControlSystem.Domain.Interfaces.Specifications.Absraction;

public interface IBaseSpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
}
