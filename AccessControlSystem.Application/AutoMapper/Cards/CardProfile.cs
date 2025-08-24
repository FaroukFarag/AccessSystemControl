using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Cards;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardDto>().ReverseMap();

        CreateMap<CreateCardDto, Card>()
            .ForMember(des => des.UnitId, opt => opt.MapFrom(src => src.Unit.Id))
            .ForMember(des => des.Unit, opt => opt.Ignore());

        CreateMap<CreateCardDto, CreateUserRequest>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.AccessLevels, opt => opt
                .MapFrom(src => src.Unit.AccessGroups!
                    .Select(ag => new UserAccessLevelRequest

                    {
                        AccessLevelId = ag.AirfobAccessLevelId,
                        SiteId = src.SiteId
                    })
                )
            );

        CreateMap<(UserDto owner, AssignOwnerToUnitDto assignDto, UnitDto unit, string cardType), CreateCardDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.owner.Id))
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.owner.SubscriptionId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.owner.UserName))
                .ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.assignDto.SiteId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.owner.Email))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.owner.PhoneNumber))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.unit))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.cardType));
    }
}
