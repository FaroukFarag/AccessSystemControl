using AccessControlSystem.Application.Dtos.Roles;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Roles;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Roles;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AccessControlSystem.Application.Services.Roles;

public class RoleService(
    IRoleRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    RoleManager<Role> roleManager) : BaseService<Role, RoleDto, int>(repository, unitOfWork, mapper), IRoleService
{
    private readonly IMapper _mapper = mapper;
    private readonly RoleManager<Role> _roleManager = roleManager;

    public override async Task<ResultDto<RoleDto>> CreateAsync(RoleDto roleDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Role",
            action: async () =>
            {
                var role = _mapper.Map<Role>(roleDto);
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Role creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return roleDto;
            });
    }

    public override async Task<ResultDto<RoleDto>> UpdateAsync(RoleDto newRoleDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Role",
            action: async () =>
            {
                var role = _mapper.Map<Role>(newRoleDto);
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Role update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return newRoleDto;
            });
    }
}
