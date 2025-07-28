using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob;

public class AirfobService(AirfobClient client) : IAirfobService
{
    protected readonly AirfobClient _client = client;

    public async Task<AirfobResponse<IEnumerable<CreateScheduleResponse>>> CreateSitesAsync(CreateSitesRequest request)
    {
        return await _client.PostAsync<CreateSitesRequest, IEnumerable<CreateScheduleResponse>>("v1/sites", request);
    }

    public async Task<AirfobResponse<IEnumerable<CreateScheduleResponse>>> CreateSchedulesAsync(CreateSchedulesRequest request)
    {
        return await _client.PostAsync<CreateSchedulesRequest, IEnumerable<CreateScheduleResponse>>("v1/schedules", request);
    }

    public async Task<AirfobResponse<IEnumerable<CreateFloorLevelResponse>>> CreateFloorLevelsAsync(CreateFloorLevelsRequest request)
    {
        return await _client.PostAsync<CreateFloorLevelsRequest, IEnumerable<CreateFloorLevelResponse>>("v1/floor_levels", request);
    }

    public async Task<AirfobResponse<GetSitesResponse>> GetSitesAsync()
    {
        return await _client.GetAsync<GetSitesResponse>("v1/sites");
    }

    public async Task<AirfobResponse<IEnumerable<int>>> GetSubSitesIdsAsync()
    {
        return await _client.GetAsync<IEnumerable<int>>("v1/sites/sub_ids");
    }

    public async Task<AirfobResponse<GetSelfAccountsResponse>> GetSelfAccountsAsync()
    {
        return await _client.GetAsync<GetSelfAccountsResponse>("v1/accounts/self");
    }

    public async Task<AirfobResponse<GetSchedulesResponse>> GetSchedulesAsync()
    {
        return await _client.GetAsync<GetSchedulesResponse>("v1/schedules");
    }

    public async Task<AirfobResponse<GetFloorLevelsResponse>> GetFloorLevelsAsync()
    {
        return await _client.GetAsync<GetFloorLevelsResponse>("v1/floor_levels");
    }

    public async Task<AirfobResponse<GetCardTemplatesResponse>> GetCardTemplatesAsync()
    {
        return await _client.GetAsync<GetCardTemplatesResponse>("v1/sites/card_templates");
    }

    public async Task<AirfobResponse<IEnumerable<CreateCardTemplateResponse>>> CreateCardTemplatesAsync(CreateCardTemplatesRequest request)
    {
        return await _client.PostAsync<CreateCardTemplatesRequest, IEnumerable<CreateCardTemplateResponse>>("v1/sites/card_templates", request);
    }

    public async Task<AirfobResponse<IEnumerable<GetRfCardResponse>>> GetRfCardsAsync(int siteId)
    {
        return await _client.GetAsync<IEnumerable<GetRfCardResponse>>($"v1/sites/{siteId}/rfcards");
    }

    public async Task<AirfobResponse<IEnumerable<GetMessageTemplateResponse>>> GetMessageTemplatesAsync(int siteId)
    {
        return await _client.GetAsync<IEnumerable<GetMessageTemplateResponse>>($"v1/sites/{siteId}/message_templates");
    }

    public async Task<AirfobResponse<IEnumerable<AssignRfCardResponse>>> AssignRfCardsAsync(AssignRfCardsRequest request)
    {
        return await _client.PostAsync<AssignRfCardsRequest, IEnumerable<AssignRfCardResponse>>($"v1/sites/{request.SiteId}/rfcards", request);
    }

    public async Task<AirfobResponse<IEnumerable<CreateMessageTemplateResponse>>> CreateMessageTemplatesAsync(CreateMessageTemplatesRequest request)
    {
        return await _client.PostAsync<CreateMessageTemplatesRequest, IEnumerable<CreateMessageTemplateResponse>>($"v1/sites/{request.SiteId}/message_templates", request);
    }
}
