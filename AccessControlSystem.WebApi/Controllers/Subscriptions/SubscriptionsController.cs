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
    [HttpPost("Create")]
    public override Task<IActionResult> Create([FromForm] SubscriptionDto dto)
    {
        return base.Create(dto);
    }

    [HttpPut("Update")]
    public override Task<IActionResult> Update([FromForm] SubscriptionDto dto)
    {
        return base.Update(dto);
    }
}
