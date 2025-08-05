using AccessControlSystem.Domain.Specifications.Absraction;

namespace AccessControlSystem.Application.Interfaces.Shared;

public interface IOrderingService<TEntity> where TEntity : class
{
    void ApplyOrdering(BaseSpecification<TEntity> specification,
        Dictionary<string, Action<BaseSpecification<TEntity>>> orderingRules,
        string orderBy);
}
