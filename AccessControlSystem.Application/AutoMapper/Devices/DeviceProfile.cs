using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Domain.Models.Devices;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Devices;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDto>().ReverseMap();
    }
}
