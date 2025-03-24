using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Subscriptions;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Subscriptions;

public class SubscriptionService(IBaseRepository<Subscription, int> repository, IUnitOfWork unitOfWork, IMapper mapper) : BaseService<Subscription, SubscriptionDto, int>(repository, unitOfWork, mapper), ISubscriptionService
{
}
