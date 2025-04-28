using AccessControlSystem.Application.Dtos.Subscriptions;
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
            .ForMember(des => des.UsedAdmins, opt => opt
                .MapFrom(src => src.Users.Count()))
            .ForMember(des => des.UsedDevices, opt => opt
                .MapFrom(src => src.SubscriptionsDevices.Count()))
            .ForMember(des => des.UsedCards, opt => opt
                .MapFrom(src => src.Cards.Count()));

        CreateMap<SubscriptionDto, Subscription>();
    }
}
