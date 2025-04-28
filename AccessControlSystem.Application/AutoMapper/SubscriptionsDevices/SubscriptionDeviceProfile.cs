using AccessControlSystem.Application.Dtos.SubscriptionsDevices;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.SubscriptionsDevices;

public class SubscriptionDeviceProfile : Profile
{
    public SubscriptionDeviceProfile()
    {
        CreateMap<SubscriptionDevice, SubscriptionDeviceDto>().ReverseMap();
    }
}
