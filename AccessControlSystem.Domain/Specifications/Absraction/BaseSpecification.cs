using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using System.Linq.Expressions;

namespace AccessControlSystem.Domain.Specifications.Absraction;

public class BaseSpecification<TEntity> : IBaseSpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; set; } = [];
    public Expression<Func<TEntity, object>>? OrderBy { get; set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }

    protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }
}
