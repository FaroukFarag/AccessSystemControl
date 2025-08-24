using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Application.Interfaces.Visitors;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Visitors;

public class VisitorService(
    IVisitorRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAirfobUserService airfobUserService) :
    BaseService<
        CreateVisitorDto, VisitorDto, VisitorDto, VisitorDto, Visitor, int>(
        repository, unitOfWork, mapper),
    IVisitorService
{
    private readonly IMapper _mapper = mapper;
    private readonly IAirfobUserService _airfobUserService = airfobUserService;

    public override async Task<ResultDto<CreateVisitorDto>> CreateAsync(CreateVisitorDto entityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Visitor Unit",
            action: async () =>
            {
                var response = await _airfobUserService.CreateUsersAsync(
                    new CreateUsersRequest
                    {
                        Users = [_mapper.Map<CreateUserRequest>(entityDto)]
                    }
                );

                if (!response.Succeeded || response.ResultData == null ||
                    !response.ResultData.Any())
                {
                    throw new InvalidOperationException("Failed to create visitor in external system");
                }
                
                entityDto.AirfobUserId = response.ResultData.FirstOrDefault()!.Id;

                return (await base.CreateAsync(entityDto)).ResultData
                    ?? throw new InvalidOperationException("Visitor creation failed");
            });
    }
}
