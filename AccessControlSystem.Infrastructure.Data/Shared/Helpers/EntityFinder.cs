using AccessControlSystem.Domain.Services.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AccessControlSystem.Infrastructure.Data.Shared.Helpers;

public class EntityFinder<TEntity, TPrimaryKey> : IEntityFinder<TEntity, TPrimaryKey>
    where TEntity : class
{
    public async Task<TEntity?> FindByIdAsync(TPrimaryKey id, IQueryable<TEntity> query)
    {
        var predicate = BuildIdPredicate(id);
        return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public Expression<Func<TEntity, bool>> BuildIdPredicate(TPrimaryKey id)
    {
        if (id is ITuple)
        {
            return CompositeKeyHelper.BuildCompositeKeyPredicate<TEntity, TPrimaryKey>(id);
        }
        else
        {
            return e => EF.Property<TPrimaryKey>(e, "Id")!.Equals(id);
        }
    }
}
