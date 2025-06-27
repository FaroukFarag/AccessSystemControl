using AccessControlSystem.Application.Dtos.UnitAccessGroups;
using AccessControlSystem.Domain.Models.UnitAccessGroups;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.UnitAccessGroups;

public class UnitAccessGroupProfile : Profile
{
    public UnitAccessGroupProfile()
    {
        CreateMap<UnitAccessGroup, UnitAccessGroupDto>().ReverseMap();
    }
}
