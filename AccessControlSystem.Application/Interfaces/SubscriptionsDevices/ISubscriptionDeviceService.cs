using AccessControlSystem.Application.Dtos.SubscriptionsDevices;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;

namespace AccessControlSystem.Application.Interfaces.SubscriptionsDevices;

public interface ISubscriptionDeviceService :
    IBaseService<
        SubscriptionDevice,
        SubscriptionDeviceDto,
        (int SubscriptionId, int DeviceId)>
{
}
