using AccessControlSystem.Application.Dtos.Roles;
using AccessControlSystem.Domain.Models.Roles;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Roles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}
