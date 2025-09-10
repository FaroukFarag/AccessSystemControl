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
    BaseService<CreateVisitorDto, VisitorDto, VisitorDto, VisitorDto, Visitor, int>(
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
                var existingVisitor = await FindExistingVisitorAsync(entityDto);

                if (existingVisitor != null)
                {
                    return await UpdateExistingVisitorAsync(existingVisitor, entityDto);
                }

                return await CreateNewVisitorAsync(entityDto);
            });
    }

    public async Task<ResultDto<bool>> SuspendVisitAsync(SuspendVisitDto suspendVisitDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Suspend Visitor",
            action: async () =>
            {
                await SuspendVisitorInExternalSystemAsync(suspendVisitDto.VisitorId);

                return true;
            });
    }

    public override async Task<ResultDto<VisitorDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Visitor",
            action: async () =>
            {
                var visitor = await GetVisitorByIdAsync(id);

                await DeleteVisitorFromExternalSystemAsync(visitor.AirfobUserId);

                return await DeleteVisitorFromDatabaseAsync(id);
            });
    }

    public override async Task<ResultDto<VisitorDto>> UpdateAsync(VisitorDto entityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Visitor",
            action: async () =>
            {
                await UpdateVisitorInExternalSystemAsync(entityDto.AirfobUserId,
                    _mapper.Map<CreateVisitorDto>(entityDto));

                return (await base.UpdateAsync(entityDto)).ResultData;
            });
    }

    private async Task<Visitor?> FindExistingVisitorAsync(CreateVisitorDto entityDto)
    {
        var specification = new BaseSpecification<Visitor>
        {
            Criteria = v => v.Name == entityDto.Name &&
                           v.Email == entityDto.Email &&
                           v.Mobile == entityDto.Mobile
        };

        var visitors = await _repository.GetAllAsync(specification);

        return visitors.FirstOrDefault();
    }

    private async Task<CreateVisitorDto> UpdateExistingVisitorAsync(Visitor existingVisitor, CreateVisitorDto entityDto)
    {
        await UpdateVisitorInExternalSystemAsync(existingVisitor.AirfobUserId, entityDto);

        existingVisitor.StartDate = entityDto.StartDate;
        existingVisitor.EndDate = entityDto.EndDate;

        var updateResponse = await base.UpdateAsync(_mapper.Map<VisitorDto>(existingVisitor));

        if (!updateResponse.Succeeded)
        {
            throw new InvalidOperationException("Failed to update existing visitor");
        }

        return entityDto;
    }

    private async Task<CreateVisitorDto> CreateNewVisitorAsync(CreateVisitorDto entityDto)
    {
        var airfobUserId = await CreateVisitorInExternalSystemAsync(entityDto);

        entityDto.AirfobUserId = airfobUserId;

        var createResult = await base.CreateAsync(entityDto);

        if (!createResult.Succeeded || createResult.ResultData == null)
        {
            await RollbackExternalVisitorCreationAsync(airfobUserId);

            throw new InvalidOperationException("Failed to create visitor in database");
        }

        return createResult.ResultData;
    }

    private async Task<int> CreateVisitorInExternalSystemAsync(CreateVisitorDto entityDto)
    {
        var createUserRequest = _mapper.Map<CreateUserRequest>(entityDto);
        var response = await _airfobUserService.CreateUsersAsync(
            new CreateUsersRequest { Users = [createUserRequest] });

        if (!response.Succeeded || response.ResultData == null || !response.ResultData.Any())
        {
            throw new InvalidOperationException("Failed to create visitor in external system");
        }

        return response.ResultData.First().Id;
    }

    private async Task UpdateVisitorInExternalSystemAsync(int airfobUserId, CreateVisitorDto entityDto)
    {
        var updateRequest = _mapper.Map<UpdateUserRequest>(entityDto);
        var response = await _airfobUserService.UpdateUserAsync(airfobUserId, updateRequest);

        if (!response.Succeeded)
        {
            throw new InvalidOperationException("Failed to update visitor in external system");
        }
    }

    private async Task SuspendVisitorInExternalSystemAsync(int visitorId)
    {
        var visitor = await GetVisitorByIdAsync(visitorId);
        var response = await _airfobUserService.SuspendUsersAsync(
            new SuspendUsersRequest { Ids = [visitor.AirfobUserId] });

        if (!response.Succeeded)
        {
            throw new InvalidOperationException("Failed to suspend visitor in external system");
        }
    }

    private async Task DeleteVisitorFromExternalSystemAsync(int airfobUserId)
    {
        var response = await _airfobUserService.DeleteUserAsync(airfobUserId);

        if (!response.Succeeded)
        {
            throw new InvalidOperationException("Failed to delete visitor from external system");
        }
    }

    private async Task RollbackExternalVisitorCreationAsync(int airfobUserId)
    {
        await _airfobUserService.DeleteUserAsync(airfobUserId);
    }

    private async Task<VisitorDto> GetVisitorByIdAsync(int id)
    {
        var getVisitorResult = await base.GetAsync(id);

        if (!getVisitorResult.Succeeded || getVisitorResult.ResultData == null)
        {
            throw new InvalidOperationException($"Visitor with ID {id} not found");
        }

        return getVisitorResult.ResultData;
    }

    private async Task<VisitorDto> DeleteVisitorFromDatabaseAsync(int id)
    {
        var deleteResult = await base.DeleteAsync(id);

        if (!deleteResult.Succeeded || deleteResult.ResultData == null)
        {
            throw new InvalidOperationException("Failed to delete visitor from database");
        }

        return deleteResult.ResultData;
    }
}
