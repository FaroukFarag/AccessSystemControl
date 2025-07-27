using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Doors;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Doors;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob.Doors;

public class AirfobDoorService(AirfobClient client) : IAirfobDoorService
{
    private readonly AirfobClient _client = client;

    public async Task<AirfobResponse<IEnumerable<CreateDoorsResponse>>> CreateDoorsAsync(CreateDoorsRequest request)
    {
        return await _client.PostAsync<CreateDoorsRequest, IEnumerable<CreateDoorsResponse>>("v1/doors", request);
    }

    public async Task<AirfobResponse<IEnumerable<AssignDevicesToDoorsResponse>>> AssignDevicesToDoorsAsync(AssignDevicesToDoorsRequest request)
    {
        return await _client.PostAsync<AssignDevicesToDoorsRequest, IEnumerable<AssignDevicesToDoorsResponse>>("v1/doors/members", request);
    }

    public async Task<AirfobResponse<IEnumerable<AssignAccessLevelsByDoorsResponse>>> AssignAccessLevelsByDoorsAsync(AssignAccessLevelsByDoorsRequest request)
    {
        return await _client.PostAsync<AssignAccessLevelsByDoorsRequest, IEnumerable<AssignAccessLevelsByDoorsResponse>>("v1/doors/access_levels/members", request);
    }

    public async Task<AirfobResponse<GetDoorsResponse>> GetDoorsAsync()
    {
        return await _client.GetAsync<GetDoorsResponse>("v1/doors");
    }
}
