using AccessControlSystem.Application.Common.Utilities;
using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Resolvers;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;
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
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<DeviceDto, Device>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<DeviceDto, CreateDeviceRequest>()
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => "AE-MU"))
            .ForMember(dest => dest.ModelType, opt => opt.MapFrom(src => "XPASS2"))
            .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => "Asia/Riyadh"));
    }
}
