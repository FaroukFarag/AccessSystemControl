using System.Linq.Expressions;

public interface IBaseSpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    List<(Expression<Func<TEntity, IEnumerable<object>>> Path, Expression<Func<object, object>> ThenInclude)> IncludesThen { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
}
