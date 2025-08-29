using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Cards;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Cards;
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
