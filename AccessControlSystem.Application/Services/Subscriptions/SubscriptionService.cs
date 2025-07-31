using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Application.Services.Shared;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Constants.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Subscriptions;

public class SubscriptionService(
    ISubscriptionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) : BaseService<Subscription, SubscriptionDto, int>(repository, unitOfWork, mapper), ISubscriptionService
{
    private readonly ISubscriptionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
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

    public override async Task<ResultDto<SubscriptionDto>> CreateAsync(SubscriptionDto subscriptionDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Subscription",
            action: async () =>
            {
                subscriptionDto.ImagePath = await _imageService.SaveImageAsync(
                    subscriptionDto.ImageFile,
                    SubscriptionConstants.SubFolder) ?? throw new InvalidOperationException("Image upload failed");

                return (await base.CreateAsync(subscriptionDto)).ResultData
                    ?? throw new InvalidOperationException("Subscription creation failed");
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
            operationName: "Get Subscription",
            action: async () =>
            {
                var specification = CreateOrderingSpecification(orderBy);
                var subscriptions = await _repository.GetAllAsync(specification);

                return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
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

    private static readonly Dictionary<string, Action<BaseSpecification<Subscription>>> OrderingRules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = spec => spec.OrderBy = s => s.CustomerName,
        ["subscription"] = spec => spec.OrderBy = s => s.SubscriptionType,
        ["recent"] = spec => spec.OrderByDescending = s => s.CreatedAt,
    };

    private static BaseSpecification<Subscription> CreateOrderingSpecification(string orderBy)
    {
        var specification = new BaseSpecification<Subscription>();
        var orderKey = string.IsNullOrWhiteSpace(orderBy) ? "recent" : orderBy;

        if (OrderingRules.TryGetValue(orderKey, out var applyOrder))
            applyOrder(specification);

        else
            specification.OrderByDescending = s => s.CreatedAt;

        return specification;
    }
}
