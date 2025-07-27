using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.AccessGroups;

public class AccessGroupProfile : Profile
{
    public AccessGroupProfile()
    {
        CreateMap<AccessGroup, AccessGroupDto>()
            .ForMember(des => des.Devices, opt => opt
                .MapFrom(src => src.AccessGroupDevices.Select(agd => agd.Device)));

        CreateMap<AccessGroupDto, AccessGroup>()
            .ForMember(des => des.AccessGroupDevices, opt => opt
                .MapFrom(src => src.Devices!
                    .Select(agd => new AccessGroupDevice
                    {
                        DeviceId = agd.Id
                    })
                )
            );

        CreateMap<AccessGroupDto, CreateAccessLevelRequest>();
    }
}
