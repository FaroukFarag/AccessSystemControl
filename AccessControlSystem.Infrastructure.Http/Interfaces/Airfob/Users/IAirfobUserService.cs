using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;

public interface IAirfobUserService
{
    Task<AirfobResponse<IEnumerable<CreateUserResponse>>> CreateUsersAsync(CreateUsersRequest request);
    Task<AirfobResponse<IEnumerable<CreateUserGroupResponse>>> CreateUserGroupsAsync(CreateUserGroupsRequest request);
    Task<AirfobResponse<IEnumerable<AssignUserGroupMemberResponse>>> AssignUserGroupMembersAsync(AssignUserGroupMembersRequest request);
    Task<AirfobResponse<IEnumerable<AssignAccessLevelsByUsersResponse>>> AssignAccessLevelsByUsersAsync(AssignAccessLevelsByUsersRequest request);
    Task<AirfobResponse<GetUsersResponse>> GetUsersAsync();
    Task<AirfobResponse<GetUserGroupsResponse>> GetUserGroupsAsync();
}
