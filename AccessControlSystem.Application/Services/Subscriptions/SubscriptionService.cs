using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Constants.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Subscriptions;

public class SubscriptionService(
    ISubscriptionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) :
    BaseService<Subscription, SubscriptionDto, int>(repository, unitOfWork, mapper), ISubscriptionService
{
    private readonly ISubscriptionRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;

    public override async Task<SubscriptionDto> CreateAsync(SubscriptionDto subscriptionDto)
    {
        subscriptionDto.ImagePath = await _imageService
            .SaveImageAsync(subscriptionDto.ImageFile, SubscriptionConstants.SubFolder);

        return await base.CreateAsync(subscriptionDto);
    }

    public override async Task<SubscriptionDto> GetAsync(int id)
    {
        var subscription = await _repository.GetAsync(id, new BaseSpecification<Subscription>
        {
            Includes = [
                s => s.Users,
                s => s.SubscriptionsDevices,
                s => s.Cards
            ],
            IncludesThen = [
                (s => s.Users, u => (u as User)!.UserRoles),
                (s => s.SubscriptionsDevices, sd => (sd as SubscriptionDevice)!.Device)
            ]
        });
        var subscriptionDto = _mapper.Map<SubscriptionDto>(subscription);

        return subscriptionDto;
    }

    public override async Task<IEnumerable<SubscriptionDto>> GetAllAsync()
    {
        var subscriptions = await base.GetAllAsync();

        return subscriptions;
    }

    public override async Task<SubscriptionDto> UpdateAsync(SubscriptionDto newSubscriptionDto)
    {
        var subscription = await GetAsync(newSubscriptionDto.Id);

        _imageService.DeleteImageAsync(subscription.ImagePath!);

        newSubscriptionDto.ImagePath = await _imageService
            .SaveImageAsync(newSubscriptionDto.ImageFile, SubscriptionConstants.SubFolder);

        return await base.UpdateAsync(newSubscriptionDto);
    }

    public override async Task<SubscriptionDto> DeleteAsync(int id)
    {
        var subscription = await GetAsync(id);

        _imageService.DeleteImageAsync(subscription.ImagePath!);

        return await base.DeleteAsync(id);
    }
}
