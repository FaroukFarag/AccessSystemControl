using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Domain.Models.Visitors;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Visitors;

public class VisitorProfile : Profile
{
    public VisitorProfile()
    {
        CreateMap<Visitor, VisitorDto>().ReverseMap();
    }
}