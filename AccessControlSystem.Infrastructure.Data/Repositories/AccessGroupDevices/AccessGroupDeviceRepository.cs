using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupDevices;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.AccessGroupDevices;

public class AccessGroupDeviceRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<AccessGroupDevice> specificationCombiner) :
    BaseRepository<AccessGroupDevice, (int AccessGroupId, int DeviceId)>(
        context,
        specificationCombiner),
    IAccessGroupDeviceRepository
{
}
