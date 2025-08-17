using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.EventLogs;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.EventLogs;

public interface IAirfobEventLogService
{
    Task<AirfobResponse<GetEventLogsResponse>> GetEventLogsAsync();
    Task<AirfobResponse<SearchEventLogsResponse>> SearchEventLogsAsync(SearchEventLogsRequest request);
}
