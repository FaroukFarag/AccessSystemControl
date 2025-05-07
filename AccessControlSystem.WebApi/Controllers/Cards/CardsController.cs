using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Cards;

[Route("api/[controller]")]
[ApiController]
public class CardsController(ICardService service) :
    BaseController<ICardService, Card, CardDto, int>(service)
{
}
