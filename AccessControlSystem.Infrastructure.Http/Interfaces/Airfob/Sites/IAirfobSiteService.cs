using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Sites;

public interface IAirfobSiteService
{
    Task<AirfobResponse<GetSitesResponse>> GetSitesAsync();
    Task<AirfobResponse<IEnumerable<int>>> GetSubSitesIdsAsync();
    Task<AirfobResponse<GetCardTemplatesResponse>> GetCardTemplatesAsync();
    Task<AirfobResponse<IEnumerable<GetRfCardResponse>>> GetRfCardsAsync(int siteId);
    Task<AirfobResponse<IEnumerable<GetMessageTemplateResponse>>> GetMessageTemplatesAsync(int siteId);
    Task<AirfobResponse<IEnumerable<CreateScheduleResponse>>> CreateSitesAsync(CreateSitesRequest request);
    Task<AirfobResponse<IEnumerable<CreateCardTemplateResponse>>> CreateCardTemplatesAsync(CreateCardTemplatesRequest request);
    Task<AirfobResponse<IEnumerable<AssignRfCardResponse>>> AssignRfCardsAsync(AssignRfCardsRequest request);
    Task<AirfobResponse<IEnumerable<CreateMessageTemplateResponse>>> CreateMessageTemplatesAsync(CreateMessageTemplatesRequest request);
}
