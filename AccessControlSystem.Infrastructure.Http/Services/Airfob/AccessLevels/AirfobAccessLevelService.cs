using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob.AccessLevels;

public class AirfobAccessLevelService(AirfobClient client) : IAirfobAccessLevelService
{
    private readonly AirfobClient _client = client;

    public async Task<AirfobResponse<IEnumerable<CreateAccessLevelResponse>>> CreateAccessLevelsAsync(CreateAccessLevelsRequest request)
    {
        return await _client.PostAsync<CreateAccessLevelsRequest, IEnumerable<CreateAccessLevelResponse>>("v1/access_levels", request);
    }

    public async Task<AirfobResponse<GetAccessLevelsResponse>> GetAccessLevelsAsync()
    {
        return await _client.GetAsync<GetAccessLevelsResponse>("v1/access_levels");
    }
}
