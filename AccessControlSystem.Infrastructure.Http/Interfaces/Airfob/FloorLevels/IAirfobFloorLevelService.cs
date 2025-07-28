using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.FloorLevels;

public interface IAirfobFloorLevelService
{
    Task<AirfobResponse<GetFloorLevelsResponse>> GetFloorLevelsAsync();
    Task<AirfobResponse<IEnumerable<CreateFloorLevelResponse>>> CreateFloorLevelsAsync(CreateFloorLevelsRequest request);
}
