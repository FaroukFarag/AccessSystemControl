using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.AccessLevels;

public interface IAirfobAccessLevelService
{
    Task<AirfobResponse<IEnumerable<CreateAccessLevelResponse>>> CreateAccessLevelsAsync(CreateAccessLevelsRequest request);
    Task<AirfobResponse<GetAccessLevelsResponse>> GetAccessLevelsAsync();
}
