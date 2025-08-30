using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Constants.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Services.Subscriptions;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Services.Subscriptions;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Application.Services.Subscriptions;

public class SubscriptionService(
    ISubscriptionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IOrderingService<Subscription> orderingService,
    SubscriptionValidationStrategyFactory strategyFactory) : BaseService<
        SubscriptionDto, SubscriptionDto, SubscriptionDto,
        SubscriptionDto, Subscription, int>(
        repository, unitOfWork, mapper), ISubscriptionService
{
    private readonly ISubscriptionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IOrderingService<Subscription> _orderingService = orderingService;
    private readonly SubscriptionValidationStrategyFactory _strategyFactory = strategyFactory;
    private static readonly BaseSpecification<Subscription> SubscriptionWithIncludesSpec = new()
    {
        Includes = [s => s.Devices, s => s.Cards],
        IncludeChains =
        [
            new IncludeChain<Subscription>
            {
                InitialInclude = s => s.Users,
                ThenIncludes = [u => (u as User)!.UserRoles]
            }
        ]
    };
    private static readonly Dictionary<string, Action<BaseSpecification<Subscription>>> OrderingRules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = spec => spec.OrderBy = s => s.CustomerName,
        ["subscription"] = spec => spec.OrderBy = s => s.SubscriptionType,
        ["recent"] = spec => spec.OrderByDescending = s => s.CreatedAt,
    };

    public override async Task<ResultDto<SubscriptionDto>> CreateAsync(SubscriptionDto subscriptionDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Subscription",
            action: async () =>
            {
                await ValidateAndProcessSubscriptionAsync(subscriptionDto);

                return await CreateSubscriptionAsync(subscriptionDto);
            });
    }

    public override async Task<ResultDto<SubscriptionDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Subscription",
            action: async () =>
            {
                var subscription = await _repository.GetAsync(id, SubscriptionWithIncludesSpec);

                return _mapper.Map<SubscriptionDto>(subscription);
            });
    }

    public async Task<ResultDto<IEnumerable<SubscriptionDto>>> GetAllAsync(string orderBy = "Recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Subscriptions",
            action: async () =>
            {
                var specification = new BaseSpecification<Subscription>();

                _orderingService.ApplyOrdering(specification, OrderingRules, orderBy);

                var subscriptions = await _repository.GetAllAsync(specification);

                return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
            });
    }

    public async Task<ResultDto<long>> GetSubscriptionsCountAsync(bool isLastMonth = false)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Subscriptions Count",
            action: async () =>
            {
                if (!isLastMonth)
                    return await _repository.GetCountAsync();

                BaseSpecification<Subscription> specification = new()
                {
                    Criteria = d => d.CreatedAt.Month < DateTime.Now.Month
                };

                return await _repository.GetCountAsync(specification);
            });
    }

    public override async Task<ResultDto<SubscriptionDto>> UpdateAsync(SubscriptionDto newSubscriptionDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Subscription",
            action: async () =>
            {
                var existingSubscription = await base.GetAsync(newSubscriptionDto.Id);
                var existingImagePath = existingSubscription.ResultData?.ImagePath;

                newSubscriptionDto.ImagePath = await _imageService.SaveImageAsync(
                    newSubscriptionDto.ImageFile,
                    SubscriptionConstants.SubFolder) ?? throw new InvalidOperationException("Image upload failed");

                var updateResult = await base.UpdateAsync(newSubscriptionDto);

                if (!string.IsNullOrEmpty(existingImagePath))
                {
                    _imageService.DeleteImage(existingImagePath);
                }

                return updateResult.ResultData
                    ?? throw new InvalidOperationException("Subscription update failed");
            });
    }

    public async Task<ResultDto<SubscriptionDto>> UpgradeAsync(UpgradeSubscriptionDto upgradeSubscriptionDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Upgrade Subscription",
            action: async () =>
            {
                var existingSubscriptionResult = await base.GetAsync(upgradeSubscriptionDto.Id);
                var existingSubscription = existingSubscriptionResult.ResultData;

                existingSubscription.SubscriptionType = upgradeSubscriptionDto.SubscriptionType;
                existingSubscription.StartDate = upgradeSubscriptionDto.StartDate;
                existingSubscription.MonthNumber = upgradeSubscriptionDto.MonthNumber;

                var updateResult = await base.UpdateAsync(existingSubscriptionResult.ResultData);

                return updateResult.ResultData
                    ?? throw new InvalidOperationException("Subscription upgrade failed");
            });
    }

    public override async Task<ResultDto<SubscriptionDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Subscription",
            action: async () =>
            {
                var subscription = await base.GetAsync(id);

                if (subscription.ResultData?.ImagePath is not null)
                {
                    _imageService.DeleteImage(subscription.ResultData.ImagePath);
                }

                return (await base.DeleteAsync(id)).ResultData
                    ?? throw new InvalidOperationException("Subscription deletion failed");
            });
    }

    private async Task ValidateAndProcessSubscriptionAsync(SubscriptionDto subscriptionDto)
    {
        subscriptionDto.ImagePath = await SaveSubscriptionImageAsync(subscriptionDto.ImageFile!);
        ValidateSubscriptionNumbers(subscriptionDto);
    }

    private async Task<string> SaveSubscriptionImageAsync(IFormFile imageFile)
    {
        return await _imageService.SaveImageAsync(
            imageFile,
            SubscriptionConstants.SubFolder)
            ?? throw new InvalidOperationException("Image upload failed");
    }

    private void ValidateSubscriptionNumbers(SubscriptionDto subscriptionDto)
    {
        var strategy = _strategyFactory.GetStrategy(subscriptionDto.SubscriptionType);

        ValidateNumber(strategy, subscriptionDto.AdminNumber, "Admin number");
        ValidateNumber(strategy, subscriptionDto.DeviceNumber, "Device number");
        ValidateNumber(strategy, subscriptionDto.CardNumber, "Card number");
    }

    private static void ValidateNumber(ISubscriptionValidationStrategy strategy, int number, string numberType)
    {
        if (!strategy.IsValid(number))
        {
            throw new InvalidOperationException(
                $"{numberType} is not valid for the selected subscription type");
        }
    }

    private async Task<SubscriptionDto> CreateSubscriptionAsync(SubscriptionDto subscriptionDto)
    {
        var result = await base.CreateAsync(subscriptionDto);
        return result.ResultData
            ?? throw new InvalidOperationException("Subscription creation failed");
    }
}
