using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Constants.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Subscriptions;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Subscriptions;

public class SubscriptionService(
    IBaseRepository<Subscription, int> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) :
    BaseService<Subscription, SubscriptionDto, int>(repository, unitOfWork, mapper), ISubscriptionService
{
    private readonly IImageService _imageService = imageService;

    public override async Task<SubscriptionDto> CreateAsync(SubscriptionDto subscriptionDto)
    {
        subscriptionDto.ImagePath = await _imageService
            .SaveImageAsync(subscriptionDto.ImageFile, SubscriptionConstants.SubFolder);

        return await base.CreateAsync(subscriptionDto);
    }

    public override async Task<SubscriptionDto> GetAsync(int id)
    {
        var subscription = await base.GetAsync(id);

        if (!string.IsNullOrEmpty(subscription.ImagePath) && File.Exists(subscription.ImagePath))
        {
            var imageBytes = await File.ReadAllBytesAsync(subscription.ImagePath);

            subscription.ImageEncode = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
        }

        return subscription;
    }

    public override async Task<IEnumerable<SubscriptionDto>> GetAllAsync()
    {
        var subscriptions = await base.GetAllAsync();

        foreach (var subscription in subscriptions)
        {
            if (!string.IsNullOrEmpty(subscription.ImagePath) && File.Exists(subscription.ImagePath))
            {
                var imageBytes = await File.ReadAllBytesAsync(subscription.ImagePath);

                subscription.ImageEncode = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        return subscriptions;
    }

    public override async Task<SubscriptionDto> Update(SubscriptionDto newSubscriptionDto)
    {
        var subscription = await GetAsync(newSubscriptionDto.Id);

        _imageService.DeleteImageAsync(subscription.ImagePath!);

        newSubscriptionDto.ImagePath = await _imageService
            .SaveImageAsync(newSubscriptionDto.ImageFile, SubscriptionConstants.SubFolder);

        return await base.Update(newSubscriptionDto);
    }

    public override async Task<SubscriptionDto> Delete(int id)
    {
        var subscription = await GetAsync(id);

        _imageService.DeleteImageAsync(subscription.ImagePath!);

        return await base.Delete(id);
    }
}
