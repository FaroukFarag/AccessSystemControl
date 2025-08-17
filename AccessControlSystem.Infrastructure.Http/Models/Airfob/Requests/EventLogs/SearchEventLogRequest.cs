namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.EventLogs;

public class SearchEventLogRequest
{
    public string Field { get; set; } = default!;
    public string Gte { get; set; } = default!;
    public string Lte { get; set; } = default!;
}
