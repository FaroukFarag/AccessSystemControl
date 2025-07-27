using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Schedules;

public interface IAirfobScheduleService
{
    Task<AirfobResponse<IEnumerable<CreateScheduleResponse>>> CreateSchedulesAsync(CreateSchedulesRequest request);
    Task<AirfobResponse<GetSchedulesResponse>> GetSchedulesAsync();
}
