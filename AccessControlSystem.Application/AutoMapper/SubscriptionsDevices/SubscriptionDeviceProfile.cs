using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Dtos.SubscriptionsDevices;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.SubscriptionsDevices;

public class SubscriptionDeviceProfile : Profile
{
    public SubscriptionDeviceProfile()
    {
        CreateMap<SubscriptionDevice, SubscriptionDeviceDto>().ReverseMap();

        CreateMap<SubscriptionDevice, DeviceDto>()
           .IncludeMembers(src => src.Device)
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeviceId));
    }
}
