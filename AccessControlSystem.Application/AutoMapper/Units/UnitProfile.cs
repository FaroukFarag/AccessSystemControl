using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Resolvers;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Units;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Units;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<Unit, UnitDto>()
            .ForMember(des => des.SubscriptionCustomerName, opt => opt
                .MapFrom(src => src.Subscription.CustomerName))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>())
            .ForMember(des => des.AccessGroups, opt => opt
                .MapFrom(src => src.AccessGroupUnits.Select(agd => agd.AccessGroup)));

        CreateMap<UnitDto, Unit>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>())
            .ForMember(des => des.AccessGroupUnits, opt => opt
                .MapFrom(src => src.AccessGroups!
                    .Select(ag => new AccessGroupUnit
                    {
                        AccessGroupId = ag.Id
                    })
                )
            );
    }
}
