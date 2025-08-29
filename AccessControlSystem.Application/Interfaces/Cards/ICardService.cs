using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Cards;

namespace AccessControlSystem.Application.Interfaces.Cards;

public interface ICardService : IBaseService<CreateCardDto, CardDto, CardDto,
    CardDto, Card, int>
{
    Task<ResultDto<IEnumerable<GetUnitCardDto>>> GetUnitCardsAsync(int unitId);
    Task<ResultDto<bool>> PauseCardAsync(PauseCardDto pauseCardDto);
    Task<ResultDto<bool>> EnableCardAsync(EnableCardDto enableCardDto);
}
