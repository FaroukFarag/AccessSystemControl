using AccessControlSystem.Application.Dtos.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.AccessGroupDevices;

public class AccessGroupDeviceProfile : Profile
{
    public AccessGroupDeviceProfile()
    {
        CreateMap<AccessGroupDevice, AccessGroupDeviceDto>().ReverseMap();
    }
}
