using AccessControlSystem.Application.Common.Utilities;
using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Resolvers;
using AccessControlSystem.Domain.Models.Devices;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Devices;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDto>()
            .ForMember(des => des.DeviceTypeName, opt => opt
                .MapFrom(src => EnumHelper.GetDescription(src.DeviceType)))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlResolver>());

        CreateMap<DeviceDto, Device>();
    }
}
