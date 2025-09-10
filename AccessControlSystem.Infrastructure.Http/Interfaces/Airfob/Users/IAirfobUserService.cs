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
    Task<AirfobResponse<InviteUserResponse>> InviteUserAsync(InviteUserRequest request);
    Task<AirfobResponse<GetUsersResponse>> GetUsersAsync();
    Task<AirfobResponse<GetUserGroupsResponse>> GetUserGroupsAsync();
    Task<AirfobResponse<SearchUsersResponse>> SearchUsersAsync(SearchUsersRequest request);
    Task<AirfobResponse<UpdateUserResponse>> UpdateUserAsync(int id, UpdateUserRequest request);
    Task<AirfobResponse<IEnumerable<SuspendUserResponse>>> SuspendUsersAsync(SuspendUsersRequest request);
    Task<AirfobResponse<IEnumerable<ActivateUserResponse>>> ActivateUsersAsync(ActivateUsersRequest request);
    Task<AirfobResponse<IEnumerable<ReactivateUserResponse>>> ReactivateUsersAsync(ReactivateUsersRequest request);
    Task<AirfobResponse<int>> DeleteUserAsync(int id);
}
