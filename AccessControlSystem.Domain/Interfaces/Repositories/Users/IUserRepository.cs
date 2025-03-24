using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Users;

public interface IUserRepository : IBaseRepository<User, int>
{
}
