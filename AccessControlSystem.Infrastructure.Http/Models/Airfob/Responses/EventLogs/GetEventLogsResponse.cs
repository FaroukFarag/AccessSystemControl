using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;

public class GetEventLogsResponse
{
    public int Total { get; set; }

    [JsonPropertyName("event_logs")]
    public IEnumerable<GetEventLogResponse> EventLogs { get; set; } = default!;
}
