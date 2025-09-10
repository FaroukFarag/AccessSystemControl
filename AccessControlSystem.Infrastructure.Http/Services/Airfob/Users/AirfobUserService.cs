using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob.Users;

public class AirfobUserService(AirfobClient client) : IAirfobUserService
{
    private readonly AirfobClient _client = client;

    public async Task<AirfobResponse<IEnumerable<CreateUserResponse>>> CreateUsersAsync(CreateUsersRequest request)
    {
        return await _client.PostAsync<CreateUsersRequest, IEnumerable<CreateUserResponse>>("v1/users", request);
    }

    public async Task<AirfobResponse<IEnumerable<CreateUserGroupResponse>>> CreateUserGroupsAsync(CreateUserGroupsRequest request)
    {
        return await _client.PostAsync<CreateUserGroupsRequest, IEnumerable<CreateUserGroupResponse>>("v1/users/groups", request);
    }

    public async Task<AirfobResponse<IEnumerable<AssignUserGroupMemberResponse>>> AssignUserGroupMembersAsync(AssignUserGroupMembersRequest request)
    {
        return await _client.PostAsync<AssignUserGroupMembersRequest, IEnumerable<AssignUserGroupMemberResponse>>("v1/users/groups/members", request);
    }

    public async Task<AirfobResponse<IEnumerable<AssignAccessLevelsByUsersResponse>>> AssignAccessLevelsByUsersAsync(AssignAccessLevelsByUsersRequest request)
    {
        return await _client.PostAsync<AssignAccessLevelsByUsersRequest, IEnumerable<AssignAccessLevelsByUsersResponse>>("v1/users/access_levels/members", request);
    }

    public async Task<AirfobResponse<InviteUserResponse>> InviteUserAsync(InviteUserRequest request)
    {
        return await _client.PostAsync<InviteUserRequest, InviteUserResponse>("v1/users/invite", request);
    }

    public async Task<AirfobResponse<GetUsersResponse>> GetUsersAsync()
    {
        return await _client.GetAsync<GetUsersResponse>("v1/users");
    }

    public async Task<AirfobResponse<GetUserGroupsResponse>> GetUserGroupsAsync()
    {
        return await _client.GetAsync<GetUserGroupsResponse>("v1/users/groups");
    }

    public async Task<AirfobResponse<SearchUsersResponse>> SearchUsersAsync(SearchUsersRequest request)
    {
        return await _client.PostAsync<SearchUsersRequest, SearchUsersResponse>("v1/users/search", request);
    }

    public async Task<AirfobResponse<UpdateUserResponse>> UpdateUserAsync(int id, UpdateUserRequest request)

    {
        return await _client.PatchAsync<UpdateUserRequest, UpdateUserResponse>($"v1/users/{id}", request);
    }

    public async Task<AirfobResponse<IEnumerable<SuspendUserResponse>>> SuspendUsersAsync(SuspendUsersRequest request)
    {
        return await _client.PostAsync<SuspendUsersRequest, IEnumerable<SuspendUserResponse>>("v1/users/suspend", request);
    }

    public async Task<AirfobResponse<IEnumerable<ActivateUserResponse>>> ActivateUsersAsync(ActivateUsersRequest request)
    {
        return await _client.PostAsync<ActivateUsersRequest, IEnumerable<ActivateUserResponse>>("v1/users/activate", request);
    }

    public async Task<AirfobResponse<IEnumerable<ReactivateUserResponse>>> ReactivateUsersAsync(ReactivateUsersRequest request)
    {
        return await _client.PostAsync<ReactivateUsersRequest, IEnumerable<ReactivateUserResponse>>("v1/users/reactivate", request);
    }

    public async Task<AirfobResponse<int>> DeleteUserAsync(int id)
    {
        return await _client.DeleteAsync<int>($"v1/users/{id}");
    }
}
