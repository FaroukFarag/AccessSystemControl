namespace AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;

public interface IWriteRepository<TEntity, TPrimaryKey> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);

    TEntity Update(TEntity newEntity);

    TEntity Delete(TPrimaryKey id);

    void DeleteRange(IEnumerable<TEntity> entities);
}
