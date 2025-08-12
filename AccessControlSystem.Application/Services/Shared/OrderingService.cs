using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Specifications.Absraction;

namespace AccessControlSystem.Application.Services.Shared;

public class OrderingService<TEntity> : IOrderingService<TEntity> where TEntity : class
{
    public void ApplyOrdering(
        BaseSpecification<TEntity> specification,
        Dictionary<string, Action<BaseSpecification<TEntity>>> orderingRules,
        string orderBy)
    {
        var orderKey = string.IsNullOrWhiteSpace(orderBy) ? "recent" : orderBy.ToLower();

        ArgumentNullException.ThrowIfNull(specification);

        specification.OrderBy = default!;
        specification.OrderByDescending = default!;

        if (orderingRules.TryGetValue(orderKey, out var applyOrder))
        {
            applyOrder(specification);
        }

        else
        {
            if (typeof(IAuditable).IsAssignableFrom(typeof(TEntity)))
            {
                specification.OrderByDescending = e => ((IAuditable)e).CreatedAt;
            }

            else
            {
                throw new InvalidOperationException(
                    $"No default ordering defined for type {typeof(TEntity).Name} and no matching order rule for '{orderBy}'");
            }
        }
    }
}
