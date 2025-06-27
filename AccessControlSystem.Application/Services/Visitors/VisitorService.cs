using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Application.Interfaces.Visitors;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Visitors;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Visitors;

public class VisitorService(
    IVisitorRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<Visitor, VisitorDto, int>(repository, unitOfWork, mapper),
    IVisitorService
{
}
