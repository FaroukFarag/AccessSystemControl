using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Infrastructure.Data.Context;

namespace AccessControlSystem.Infrastructure.Data.UnitOfWork;

public class UnitOfWork(AccessControlDbContext context) : IUnitOfWork
{
    private readonly AccessControlDbContext _context = context;

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
