namespace AccessControlSystem.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> Complete();
}