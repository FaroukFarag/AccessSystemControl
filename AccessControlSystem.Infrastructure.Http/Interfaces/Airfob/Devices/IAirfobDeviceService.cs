using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Devices;

public interface IAirfobDeviceService
{
    Task<AirfobResponse<IEnumerable<CreateDevicesResponse>>> CreateDevicesAsync(CreateDevicesRequest request);
    Task<AirfobResponse<IEnumerable<AssignAccessLevelByDeviceResponse>>> AssignAccessLevelsByDevices(AssignAccessLevelsByDevicesRequest request);
    Task<AirfobResponse<GetDevicesResponse>> GetDevicesAsync();
}
