using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroups;

namespace AccessControlSystem.Application.Interfaces.AccessGroups;

public interface IAccessGroupService : IBaseService<AccessGroupDto,
    GetAllAccessGroupsDto, AccessGroupDto, AccessGroupDto, AccessGroup, int>
{
    Task<ResultDto<IEnumerable<GetAllAccessGroupsDto>>> GetAllAsync(string orderBy);
}
