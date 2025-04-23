namespace AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;

public interface IBaseRepository<TEntity, TPrimaryKey> :
    IReadRepository<TEntity, TPrimaryKey>,
    IWriteRepository<TEntity, TPrimaryKey>,
    IAggregateRepository<TEntity, TPrimaryKey>
    where TEntity : class
{
}