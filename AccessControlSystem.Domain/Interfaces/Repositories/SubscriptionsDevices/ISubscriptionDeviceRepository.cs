using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;

namespace AccessControlSystem.Domain.Interfaces.Repositories.SubscriptionsDevices;

public interface ISubscriptionDeviceRepository :
    IBaseRepository<SubscriptionDevice, (int SubscriptionId, int DeviceId)>
{
}
