using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Cards;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Domain.Specifications.Absraction;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Cards;

public class CardService(
    ICardRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAirfobUserService airfobUserService) :
    BaseService<CreateCardDto, CardDto, CardDto, CardDto, Card, int>(
        repository, unitOfWork, mapper), ICardService
{
    private readonly ICardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IAirfobUserService _airfobUserService = airfobUserService;

    public async override Task<ResultDto<CreateCardDto>> CreateAsync(CreateCardDto createEntityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Card",
            action: async () =>
            {
                var response = await _airfobUserService.CreateUsersAsync(new CreateUsersRequest
                {
                    Users = [_mapper.Map<CreateUserRequest>(createEntityDto)]
                });

                if (!response.Succeeded || response.ResultData == null ||
                    !response.ResultData.Any())
                {
                    throw new InvalidOperationException("Failed to create card in external system");
                }

                createEntityDto.AirfobUserId = response.ResultData.FirstOrDefault()!.Id;

                var createResult = await base.CreateAsync(createEntityDto);

                if (!createResult.Succeeded)
                    throw new InvalidOperationException("Failed to create card in database");

                return createResult.ResultData;
            });
    }

    public async Task<ResultDto<IEnumerable<GetUnitCardDto>>> GetUnitCardsAsync(int unitId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Unit Cards",
            action: async () =>
            {
                var getCardsResult = await _airfobUserService.GetUsersAsync();

                if (!getCardsResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to get unit cards in external system");
                }

                var cardsResult = await _repository.GetAllAsync(new BaseSpecification<Card>
                {
                    Criteria = c => c.UnitId == unitId
                });

                return (from airfobUser in getCardsResult.ResultData.Users
                        .Where(u => u.Type == "normal")
                        join card in cardsResult
                        on airfobUser.Id equals card.AirfobUserId
                        where airfobUser.Type == "normal"
                        select new GetUnitCardDto
                        {
                            Id = card.Id,
                            Name = airfobUser.Name,
                            Mobile = airfobUser.Mobile,
                            Email = airfobUser.Email,
                            UserId = airfobUser.Id,
                            Status = airfobUser.Status,
                        });
            });
    }

    public async Task<ResultDto<bool>> PauseCardAsync(PauseCardDto pauseCardDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Pause Card",
            action: async () =>
            {
                var response = await _airfobUserService.SuspendUsersAsync(
                    new SuspendUsersRequest
                    {
                        Ids = [pauseCardDto.CardId]
                    }
                );

                if (!response.Succeeded)
                {
                    throw new InvalidOperationException("Failed to suspend card in external system");
                }

                return true;
            });
    }

    public async Task<ResultDto<bool>> EnableCardAsync(EnableCardDto enableCardDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Enable Card",
            action: async () =>
            {
                var response = await _airfobUserService.ActivateUsersAsync(new ActivateUsersRequest
                {
                    Users = [_mapper.Map<ActivateUserRequest>(enableCardDto)]
                });

                if (!response.Succeeded || response.ResultData == null ||
                    !response.ResultData.Any())
                {
                    throw new InvalidOperationException("Failed to create card in external system");
                }

                return true;
            });
    }

    public async Task<ResultDto<bool>> RegenerateCardAsync(RegenerateCardDto regenerateCardDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Regenerate Card",
            action: async () =>
            {
                var response = await _airfobUserService.ReactivateUsersAsync(new ReactivateUsersRequest
                {
                    Users = [_mapper.Map<ReactivateUserRequest>(regenerateCardDto)]
                });

                if (!response.Succeeded || response.ResultData == null ||
                    !response.ResultData.Any())
                {
                    throw new InvalidOperationException("Failed to create card in external system");
                }

                return true;
            });
    }

    public async override Task<ResultDto<CardDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Card",
            action: async () =>
            {
                var getCardResult = await GetAsync(id);

                if (!getCardResult.Succeeded)
                    throw new InvalidOperationException("Card not found");

                var cardDto = getCardResult.ResultData;
                var response = await _airfobUserService.DeleteUserAsync(cardDto.AirfobUserId);

                if (!response.Succeeded)
                {
                    throw new InvalidOperationException("Failed to delete card in external system");
                }

                var deleteResult = await base.DeleteAsync(id);

                if (!deleteResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to delete card");
                }

                return deleteResult.ResultData;
            });
    }
}
