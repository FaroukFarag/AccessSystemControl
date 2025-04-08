using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Domain.Models.Units;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Units;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<Unit, UnitDto>().ReverseMap();
    }
}
