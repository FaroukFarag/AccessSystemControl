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

        CreateMap<CreateVisitorDto, Visitor>().ForMember(
            des => des.AccessGroups, opt => opt.Ignore());

        CreateMap<CreateVisitorDto, CreateUserRequest>()
            .ForMember(des => des.AccessLevels, opt => opt
                .MapFrom(src => src.Unit.AccessGroups!
                    .Select(ag => new UserAccessLevelRequest
                    {
                        StartDate = src.StartDate,
                        EndDate = src.EndDate,
                        AccessLevelId = ag.AirfobAccessLevelId,
                        SiteId = src.SiteId
                    })
                )
            );
    }
}