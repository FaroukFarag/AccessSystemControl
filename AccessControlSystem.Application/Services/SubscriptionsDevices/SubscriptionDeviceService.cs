using AccessControlSystem.Application.Dtos.SubscriptionsDevices;
using AccessControlSystem.Application.Interfaces.SubscriptionsDevices;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.SubscriptionsDevices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AutoMapper;

namespace AccessControlSystem.Application.Services.SubscriptionsDevices;

public class SubscriptionDeviceService(
    ISubscriptionDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        SubscriptionDevice,
        SubscriptionDeviceDto,
        (int SubscriptionId, int DeviceId)>(
        repository,
        unitOfWork,
        mapper), ISubscriptionDeviceService
{
}
