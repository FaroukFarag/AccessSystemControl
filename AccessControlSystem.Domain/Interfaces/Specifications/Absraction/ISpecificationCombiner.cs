using System.Linq.Expressions;

namespace AccessControlSystem.Domain.Interfaces.Specifications.Absraction;

public interface ISpecificationCombiner<TEntity>
{
    IBaseSpecification<TEntity> Combine(
        IBaseSpecification<TEntity>? spec,
        Expression<Func<TEntity, bool>> predicate);
}
