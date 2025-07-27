using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob.Devices;

public class AirfobDeviceService(AirfobClient client) : IAirfobDeviceService
{
    private readonly AirfobClient _client = client;

    public async Task<AirfobResponse<IEnumerable<CreateDevicesResponse>>> CreateDevicesAsync(CreateDevicesRequest request)
    {
        return await _client.PostAsync<CreateDevicesRequest, IEnumerable<CreateDevicesResponse>>("v1/devices", request);
    }

    public async Task<AirfobResponse<IEnumerable<AssignAccessLevelByDeviceResponse>>> AssignAccessLevelsByDevices(AssignAccessLevelsByDevicesRequest request)
    {
        return await _client.PostAsync<AssignAccessLevelsByDevicesRequest, IEnumerable<AssignAccessLevelByDeviceResponse>>("v1/devices/access_levels/members", request);
    }

    public async Task<AirfobResponse<GetDevicesResponse>> GetDevicesAsync()
    {
        return await _client.GetAsync<GetDevicesResponse>("v1/devices");
    }
}
