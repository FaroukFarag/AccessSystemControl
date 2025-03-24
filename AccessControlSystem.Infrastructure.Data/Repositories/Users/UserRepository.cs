using AccessControlSystem.Domain.Interfaces.Repositories.Users;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Users;
public class UserRepository(
AccessControlDbContext context,
    ISpecificationCombiner<User> specificationCombiner) :
    BaseRepository<User, int>(context, specificationCombiner), IUserRepository
{
}
