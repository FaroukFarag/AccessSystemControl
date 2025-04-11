using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Domain.Models.AccessGroups;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.AccessGroups;

public class AccessGroupProfile : Profile
{
    public AccessGroupProfile()
    {
        CreateMap<AccessGroup, AccessGroupDto>().ReverseMap();
    }
}
