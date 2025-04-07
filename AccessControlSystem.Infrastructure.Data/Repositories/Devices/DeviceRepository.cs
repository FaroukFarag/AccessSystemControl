using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Devices;

public class DeviceRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Device> specificationCombiner) :
    BaseRepository<Device, int>(context, specificationCombiner), IDeviceRepository
{
}
