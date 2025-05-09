using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Domain.Models.AccessGroups;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.AccessGroups;

public class AccessGroupProfile : Profile
{
    public AccessGroupProfile()
    {
        CreateMap<AccessGroup, AccessGroupDto>()
            .ForMember(des => des.Devices, opt => opt
                .MapFrom(src => src.AccessGroupDevices.Select(agd => agd.Device)));

        CreateMap<AccessGroupDto, AccessGroup>();
    }
}
