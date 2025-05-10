using AccessControlSystem.Application.Dtos.SubscriptionsDevices;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Interfaces.SubscriptionsDevices;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.SubscriptionsDevices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AccessControlSystem.Domain.Services.Subscriptions;
using AutoMapper;

namespace AccessControlSystem.Application.Services.SubscriptionsDevices;

public class SubscriptionDeviceService(
    ISubscriptionDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ISubscriptionService subscriptionService,
    SubscriptionValidationStrategyFactory strategyFactory) :
    BaseService<
        SubscriptionDevice,
        SubscriptionDeviceDto,
        (int SubscriptionId, int DeviceId)>(
        repository,
        unitOfWork,
        mapper), ISubscriptionDeviceService
{
    private readonly ISubscriptionService _subscriptionService = subscriptionService;
    private readonly SubscriptionValidationStrategyFactory _strategyFactory = strategyFactory;

    public async override Task<bool> CreateRangeAsync(IEnumerable<SubscriptionDeviceDto> subscriptionsDevicesDtos)
    {
        if (subscriptionsDevicesDtos == null || subscriptionsDevicesDtos.Count() == 0)
            return false;

        var subscriptionDto = await _subscriptionService
            .GetAsync(subscriptionsDevicesDtos.FirstOrDefault()!.SubscriptionId);
        var strategy = _strategyFactory.GetStrategy(subscriptionDto.SubscriptionType);

        if (!strategy.IsValid(subscriptionsDevicesDtos.Count()))
            return false;

        return await base.CreateRangeAsync(subscriptionsDevicesDtos);
    }
}
