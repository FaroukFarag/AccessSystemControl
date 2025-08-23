using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Cards;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardDto>().ReverseMap();

        CreateMap<CreateCardDto, CreateUserRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.AccessLevels, opt => opt.MapFrom(src => src.Unit.AccessGroups!.Select(ag => new UserAccessLevelRequest
                {
                    AccessLevelId = ag.AirfobAccessLevelId,
                    SiteId = src.SiteId
                }))
                );
    }
}
