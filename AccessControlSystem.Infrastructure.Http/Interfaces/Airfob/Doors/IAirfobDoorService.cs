using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Doors;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Doors;

public interface IAirfobDoorService
{
    Task<AirfobResponse<IEnumerable<CreateDoorsResponse>>> CreateDoorsAsync(CreateDoorsRequest request);
    Task<AirfobResponse<IEnumerable<AssignDevicesToDoorsResponse>>> AssignDevicesToDoorsAsync(AssignDevicesToDoorsRequest request);
    Task<AirfobResponse<IEnumerable<AssignAccessLevelsByDoorsResponse>>> AssignAccessLevelsByDoorsAsync(AssignAccessLevelsByDoorsRequest request);
    Task<AirfobResponse<GetDoorsResponse>> GetDoorsAsync();
}
