using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Visitors;

public class VisitorProfile : Profile
{
    public VisitorProfile()
    {
        CreateMap<Visitor, VisitorDto>().ReverseMap();

        CreateMap<VisitorDto, InviteUserRequest>()
            .ForMember(des => des.AccessLevelIds, opt => opt
                .MapFrom(src => src.AccessGroups.Select(ag => ag.AirfobAccessLevelId)));
    }
}