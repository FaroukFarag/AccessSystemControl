using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;

public class SearchEventLogsResponse
{
    public int Total { get; set; }

    [JsonPropertyName("event_logs")]
    public IEnumerable<SearchEventLogResponse> EventLogs { get; set; } = default!;
}
