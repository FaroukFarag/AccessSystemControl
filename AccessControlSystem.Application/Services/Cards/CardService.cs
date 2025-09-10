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
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;
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

    public override async Task<ResultDto<CreateCardDto>> CreateAsync(CreateCardDto createEntityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Card",
            action: async () =>
            {
                var airfobUserId = await CreateCardInExternalSystemAsync(createEntityDto);

                createEntityDto.AirfobUserId = airfobUserId;

                return await CreateCardInDatabaseAsync(createEntityDto);
            });
    }

    public async Task<ResultDto<IEnumerable<GetUnitCardDto>>> GetUnitCardsAsync(int unitId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Unit Cards",
            action: async () =>
            {
                var externalCards = await GetCardsFromExternalSystemAsync();
                var internalCards = await GetCardsFromDatabaseAsync(unitId);

                return MapUnitCards(externalCards, internalCards);
            });
    }

    public async Task<ResultDto<bool>> PauseCardAsync(PauseCardDto pauseCardDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Pause Card",
            action: async () =>
            {
                await SuspendCardInExternalSystemAsync(pauseCardDto.UserId);

                return true;
            });
    }

    public async Task<ResultDto<bool>> EnableCardAsync(EnableCardDto enableCardDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Enable Card",
            action: async () =>
            {
                await ActivateCardInExternalSystemAsync(enableCardDto);

                return true;
            });
    }

    public async Task<ResultDto<bool>> RegenerateCardAsync(RegenerateCardDto regenerateCardDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Regenerate Card",
            action: async () =>
            {
                await ReactivateCardInExternalSystemAsync(regenerateCardDto);

                return true;
            });
    }

    public override async Task<ResultDto<CardDto>> UpdateAsync(CardDto entityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Card",
            action: async () =>
            {
                return (await base.UpdateAsync(entityDto)).ResultData;
            });
    }

    public override async Task<ResultDto<CardDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Card",
            action: async () =>
            {
                var card = await GetCardByIdAsync(id);

                await DeleteCardFromExternalSystemAsync(card.AirfobUserId);

                return await DeleteCardFromDatabaseAsync(id);
            });
    }

    private async Task<int> CreateCardInExternalSystemAsync(CreateCardDto createEntityDto)
    {
        var createUserRequest = _mapper.Map<CreateUserRequest>(createEntityDto);
        var response = await _airfobUserService.CreateUsersAsync(new CreateUsersRequest
        {
            Users = [createUserRequest]
        });

        if (!response.Succeeded || response.ResultData == null || !response.ResultData.Any())
        {
            throw new InvalidOperationException("Failed to create card in external system");
        }

        return response.ResultData.First().Id;
    }

    private async Task<CreateCardDto> CreateCardInDatabaseAsync(CreateCardDto createEntityDto)
    {
        var createResult = await base.CreateAsync(createEntityDto);

        if (!createResult.Succeeded || createResult.ResultData == null)
        {
            await TryRollbackExternalCardCreationAsync(createEntityDto.AirfobUserId);

            throw new InvalidOperationException("Failed to create card in database");
        }

        return createResult.ResultData;
    }

    private async Task TryRollbackExternalCardCreationAsync(int airfobUserId)
    {

        await _airfobUserService.DeleteUserAsync(airfobUserId);
    }

    private async Task<GetUsersResponse> GetCardsFromExternalSystemAsync()
    {
        var getCardsResult = await _airfobUserService.GetUsersAsync();

        if (!getCardsResult.Succeeded || getCardsResult.ResultData == null)
        {
            throw new InvalidOperationException("Failed to get cards from external system");
        }

        return getCardsResult.ResultData;
    }

    private async Task<IEnumerable<Card>> GetCardsFromDatabaseAsync(int unitId)
    {
        var specification = new BaseSpecification<Card>
        {
            Criteria = c => c.UnitId == unitId
        };

        return await _repository.GetAllAsync(specification);
    }

    private IEnumerable<GetUnitCardDto> MapUnitCards(GetUsersResponse externalCards, IEnumerable<Card> internalCards)
    {
        return from airfobUser in externalCards.Users.Where(u => u.Type == "normal")
               join card in internalCards
               on airfobUser.Id equals card.AirfobUserId
               select new GetUnitCardDto
               {
                   Id = card.Id,
                   Name = airfobUser.Name,
                   Mobile = airfobUser.Mobile,
                   Email = airfobUser.Email,
                   UserId = airfobUser.Id,
                   Status = airfobUser.Status,
               };
    }

    private async Task SuspendCardInExternalSystemAsync(int userId)
    {
        var response = await _airfobUserService.SuspendUsersAsync(
            new SuspendUsersRequest { Ids = [userId] });

        if (!response.Succeeded)
        {
            throw new InvalidOperationException("Failed to suspend card in external system");
        }
    }

    private async Task ActivateCardInExternalSystemAsync(EnableCardDto enableCardDto)
    {
        var activateRequest = _mapper.Map<ActivateUserRequest>(enableCardDto);
        var response = await _airfobUserService.ActivateUsersAsync(
            new ActivateUsersRequest { Users = [activateRequest] });

        if (!response.Succeeded || response.ResultData == null || !response.ResultData.Any())
        {
            throw new InvalidOperationException("Failed to activate card in external system");
        }
    }

    private async Task ReactivateCardInExternalSystemAsync(RegenerateCardDto regenerateCardDto)
    {
        var reactivateRequest = _mapper.Map<ReactivateUserRequest>(regenerateCardDto);
        var response = await _airfobUserService.ReactivateUsersAsync(
            new ReactivateUsersRequest { Users = [reactivateRequest] });

        if (!response.Succeeded || response.ResultData == null || !response.ResultData.Any())
        {
            throw new InvalidOperationException("Failed to regenerate card in external system");
        }
    }

    private async Task<CardDto> GetCardByIdAsync(int id)
    {
        var getCardResult = await base.GetAsync(id);

        if (!getCardResult.Succeeded || getCardResult.ResultData == null)
        {
            throw new InvalidOperationException($"Card with ID {id} not found");
        }

        return getCardResult.ResultData;
    }

    private async Task DeleteCardFromExternalSystemAsync(int airfobUserId)
    {
        var response = await _airfobUserService.DeleteUserAsync(airfobUserId);

        if (!response.Succeeded)
        {
            throw new InvalidOperationException("Failed to delete card from external system");
        }
    }

    private async Task<CardDto> DeleteCardFromDatabaseAsync(int id)
    {
        var deleteResult = await base.DeleteAsync(id);

        if (!deleteResult.Succeeded || deleteResult.ResultData == null)
        {
            throw new InvalidOperationException("Failed to delete card from database");
        }

        return deleteResult.ResultData;
    }
}
