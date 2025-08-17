namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.EventLogs;

public class SearchEventLogsRequest
{
    public IEnumerable<SearchEventLogRequest> Filters { get; set; } = default!;
}
