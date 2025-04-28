using AccessControlSystem.Domain.Interfaces.Repositories.SubscriptionsDevices;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.SubscriptionsDevices;

public class SubscriptionDeviceRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<SubscriptionDevice> specificationCombiner) :
    BaseRepository<SubscriptionDevice, (int SubscriptionId, int DeviceId)>(
        context,
        specificationCombiner),
    ISubscriptionDeviceRepository
{
}
