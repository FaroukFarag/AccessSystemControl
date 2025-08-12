using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Cards;

namespace AccessControlSystem.Application.Interfaces.Cards;

public interface ICardService : IBaseService<CardDto, CardDto, CardDto,
    CardDto, Card, int>
{
}
