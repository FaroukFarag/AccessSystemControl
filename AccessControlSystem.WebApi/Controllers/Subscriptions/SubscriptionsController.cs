using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Subscriptions;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController(ISubscriptionService service) :
    BaseController<ISubscriptionService, SubscriptionDto, SubscriptionDto,
        SubscriptionDto, SubscriptionDto, Subscription, int>(service)
{
    private readonly ISubscriptionService _service = service;

    [HttpGet("GetAll/{orderBy}")]
    public async Task<IActionResult> GetAll(string orderBy)
    {
        return Ok(await _service.GetAllAsync(orderBy));
    }

    [HttpGet("GetSubscriptionsCount")]
    public async Task<IActionResult> GetSubscriptionsCount()
    {
        return Ok(await _service.GetSubscriptionsCountAsync());
    }

    [HttpGet("GetLastMonthSubscriptionsCount")]
    public async Task<IActionResult> GetLastMonthSubscriptionsCount()
    {
        return Ok(await _service.GetSubscriptionsCountAsync(true));
    }

    [HttpPut("Update")]
    public override Task<IActionResult> Update([FromForm] SubscriptionDto dto)
    {
        return base.Update(dto);
    }

    [HttpPatch("UpgradeSubscription")]
    public async Task<IActionResult> UpgradeSubscription(UpgradeSubscriptionDto dto)
    {
        return Ok(await _service.UpgradeAsync(dto));
    }
}
