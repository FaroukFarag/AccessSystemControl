using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Subscriptions;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SubscriptionsController(ISubscriptionService subscriptionService) :
    BaseController<ISubscriptionService, Subscription, SubscriptionDto, int>(subscriptionService)
{
}
