using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Subscriptions;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController(ISubscriptionService service) :
    BaseController<ISubscriptionService, Subscription, SubscriptionDto, int>(service)
{
}
