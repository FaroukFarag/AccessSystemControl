using AccessControlSystem.Application.Dtos.AccessGroupUnits;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.AccessGroupUnits;

public class AccessGroupUnitProfile : Profile
{
    public AccessGroupUnitProfile()
    {
        CreateMap<AccessGroupUnit, AccessGroupUnitDto>().ReverseMap();
    }
}
