using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Cards;

[Route("api/[controller]")]
[ApiController]
public class CardsController(ICardService service) :
    BaseController<ICardService, CreateCardDto, CardDto, CardDto, CardDto, Card,
        int>(service)
{
    private readonly ICardService _service = service;

    [HttpPut("EnableCard")]
    public virtual async Task<IActionResult> EnableCard(EnableCardDto enableCardDto)
    {
        return Ok(await _service.EnableCardAsync(enableCardDto));
    }

    [HttpPut("RegenerateCard")]
    public virtual async Task<IActionResult> RegenerateCard(RegenerateCardDto regenerateCardDto)
    {
        return Ok(await _service.RegenerateCardAsync(regenerateCardDto));
    }

    [HttpDelete("PauseCard")]
    public virtual async Task<IActionResult> PauseCard(PauseCardDto pauseCardDto)
    {
        return Ok(await _service.PauseCardAsync(pauseCardDto));
    }
}
