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
    BaseService<Visitor, VisitorDto, int>(repository, unitOfWork, mapper),
    IVisitorService
{
    private readonly IMapper _mapper = mapper;
    private readonly IAirfobUserService _airfobUserService = airfobUserService;
    public override async Task<ResultDto<VisitorDto>> CreateAsync(VisitorDto entityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Visitor Unit",
            action: async () =>
            {
                var inviteUserRequest = _mapper.Map<InviteUserRequest>(entityDto);
                var result = await _airfobUserService.InviteUserAsync(inviteUserRequest);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Visitor creation failed");

                entityDto.InviteToken = result.ResultData?.InviteToken;
                entityDto.AccessGroups = default!;

                return (await base.CreateAsync(entityDto)).ResultData
                    ?? throw new InvalidOperationException("Visitor creation failed");
            });
    }
}
