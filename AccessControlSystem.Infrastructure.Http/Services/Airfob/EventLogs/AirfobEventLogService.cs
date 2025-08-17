using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.EventLogs;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.EventLogs;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob.EventLogs;

public class AirfobEventLogService(AirfobClient client) : IAirfobEventLogService
{
    private readonly AirfobClient _client = client;

    public async Task<AirfobResponse<GetEventLogsResponse>> GetEventLogsAsync()
    {
        return await _client.GetAsync<GetEventLogsResponse>("v1/event_logs");
    }

    public async Task<AirfobResponse<SearchEventLogsResponse>> SearchEventLogsAsync(SearchEventLogsRequest request)
    {
        return await _client.PostAsync<SearchEventLogsRequest, SearchEventLogsResponse>("v1/event_logs/search", request);
    }
}
