using AccessControlSystem.Application.Dtos.Roles;
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
    RoleManager<Role> roleManager) :
    BaseService<Role, RoleDto, int>(repository, unitOfWork, mapper), IRoleService
{
    private readonly IMapper _mapper = mapper;
    private readonly RoleManager<Role> _roleManager = roleManager;

    public async override Task<RoleDto> CreateAsync(RoleDto roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);

        var result = await _roleManager.CreateAsync(role);

        return result.Succeeded ? roleDto : default!;
    }

    public async override Task<RoleDto> Update(RoleDto newRoleDto)
    {
        var role = _mapper.Map<Role>(newRoleDto);

        var result = await _roleManager.UpdateAsync(role);

        return result.Succeeded ? newRoleDto : default!;
    }
}
