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
                .MapFrom(src => src.SubscriptionType.ToString()));

        CreateMap<SubscriptionDto, Subscription>();
    }
}
