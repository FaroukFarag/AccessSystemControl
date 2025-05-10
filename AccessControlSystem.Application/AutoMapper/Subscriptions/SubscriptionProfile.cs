using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Resolvers;
using AccessControlSystem.Domain.Enums.Roles;
using AccessControlSystem.Domain.Models.Subscriptions;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Subscriptions;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<Subscription, SubscriptionDto>()
            .ForMember(des => des.SubscriptionTypeName, opt => opt
                .MapFrom(src => src.SubscriptionType.ToString()))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlResolver>())
            .ForMember(des => des.UsedAdmins, opt => opt
                .MapFrom(src => src.Users.Count(u => u.UserRoles
                    .Any(ur => ur.RoleId == (int)RoleNames.Admin))))
            .ForMember(des => des.UsedDevices, opt => opt
                .MapFrom(src => src.SubscriptionsDevices.Count()))
            .ForMember(des => des.UsedCards, opt => opt
                .MapFrom(src => src.Cards.Count()))
            .ForMember(des => des.Devices, opt => opt
                .MapFrom(src => src.SubscriptionsDevices.Select(sd => sd.Device)));

        CreateMap<SubscriptionDto, Subscription>();
    }
}
