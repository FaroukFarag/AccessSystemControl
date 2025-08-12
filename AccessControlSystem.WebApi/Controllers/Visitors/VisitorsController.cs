using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Application.Interfaces.Visitors;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Visitors;

[Route("api/[controller]")]
[ApiController]
public class VisitorsController(IVisitorService service)
    : BaseController<IVisitorService, CreateVisitorDto, VisitorDto, VisitorDto, VisitorDto, Visitor, int>(service)
{
}
