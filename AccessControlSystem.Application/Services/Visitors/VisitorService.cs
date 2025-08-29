using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Application.Interfaces.Visitors;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.Domain.Specifications.Absraction;
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
    private readonly IVisitorRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IAirfobUserService _airfobUserService = airfobUserService;

    public override async Task<ResultDto<CreateVisitorDto>> CreateAsync(CreateVisitorDto entityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Visitor",
            action: async () =>
            {
                var visitors = await _repository.GetAllAsync(new BaseSpecification<Visitor>
                {
                    Criteria = v => v.Name == entityDto.Name &&
                        v.Email == entityDto.Email && v.Mobile == entityDto.Mobile,
                });

                if (visitors.Any())
                {
                    var visitor = visitors.FirstOrDefault()!;
                    var updateExternalResponse = await _airfobUserService
                        .UpdateUserAsync(_mapper.Map<UpdateUserRequest>(entityDto));

                    if (!updateExternalResponse.Succeeded)
                        throw new InvalidOperationException("Failed to create visitor in external system");

                    visitor.StartDate = entityDto.StartDate;
                    visitor.EndDate = entityDto.EndDate;

                    var updateResponse = await base.UpdateAsync(_mapper.Map<VisitorDto>(visitor));

                    if (!updateResponse.Succeeded)
                        throw new InvalidOperationException("Visitor creation failed");

                    return entityDto;
                }

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

    public async Task<ResultDto<bool>> SuspendVisitAsync(SuspendVisitDto suspendVisitDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Suspend Visitor",
            action: async () =>
            {
                var response = await _airfobUserService.SuspendUsersAsync(
                    new SuspendUsersRequest
                    {
                        Ids = [suspendVisitDto.VisitorId]
                    }
                );

                if (!response.Succeeded)
                {
                    throw new InvalidOperationException("Failed to suspend visitor in external system");
                }

                return true;
            });
    }

    public async override Task<ResultDto<VisitorDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Visitor",
            action: async () =>
            {
                var getVisitorResult = await GetAsync(id);

                if (!getVisitorResult.Succeeded)
                    throw new InvalidOperationException("Visitor not found");

                var visitorDto = getVisitorResult.ResultData;
                var response = await _airfobUserService.DeleteUserAsync(visitorDto.AirfobUserId);

                if (!response.Succeeded)
                {
                    throw new InvalidOperationException("Failed to pause visitor in external system");
                }

                var deleteResult = await base.DeleteAsync(id);

                if (!deleteResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to delete visitor");
                }

                return deleteResult.ResultData;
            });
    }
}
