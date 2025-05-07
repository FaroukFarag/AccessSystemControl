using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Domain.Models.Cards;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Cards;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardDto>().ReverseMap();
    }
}
