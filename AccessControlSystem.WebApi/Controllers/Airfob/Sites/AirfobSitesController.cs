using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Sites;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Airfob.Sites;

[Route("api/[controller]")]
[ApiController]
public class AirfobSitesController(IAirfobSiteService service) : ControllerBase
{
    private readonly IAirfobSiteService _service = service;

    [HttpPost("Create")]
    public virtual async Task<IActionResult> Create(CreateSitesRequest request)
    {
        return Ok(await _service.CreateSitesAsync(request));
    }

    [HttpPost("CreateCardTemplates")]
    public virtual async Task<IActionResult> CreateCardTemplates(CreateCardTemplatesRequest request)
    {
        return Ok(await _service.CreateCardTemplatesAsync(request));
    }

    [HttpPost("AssignRfCards")]
    public virtual async Task<IActionResult> AssignRfCards(AssignRfCardsRequest request)
    {
        return Ok(await _service.AssignRfCardsAsync(request));
    }

    [HttpPost("CreateMessageTemplates")]
    public virtual async Task<IActionResult> CreateMessageTemplates(CreateMessageTemplatesRequest request)
    {
        return Ok(await _service.CreateMessageTemplatesAsync(request));
    }

    [HttpGet("GetAll")]
    public virtual async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetSitesAsync());
    }

    [HttpGet("GetSubSitesIds")]
    public virtual async Task<IActionResult> GetSubSitesIds()
    {
        return Ok(await _service.GetSubSitesIdsAsync());
    }

    [HttpGet("GetCardTemplates")]
    public virtual async Task<IActionResult> GetCardTemplates()
    {
        return Ok(await _service.GetCardTemplatesAsync());
    }

    [HttpGet("GetRfCards")]
    public virtual async Task<IActionResult> GetRfCards(int siteId)
    {
        return Ok(await _service.GetRfCardsAsync(siteId));
    }

    [HttpGet("GetMessageTemplates")]
    public virtual async Task<IActionResult> GetMessageTemplates(int siteId)
    {
        return Ok(await _service.GetMessageTemplatesAsync(siteId));
    }
}
