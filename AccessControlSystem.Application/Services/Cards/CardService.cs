using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Cards;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Cards;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Cards;

public class CardService(
    ICardRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<CardDto, CardDto, CardDto, CardDto, Card, int>(
        repository, unitOfWork, mapper), ICardService
{
}
