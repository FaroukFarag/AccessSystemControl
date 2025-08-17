using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;

public class GetEventLogsResponse
{
    public int Total { get; set; }

    [JsonPropertyName("event_logs")]
    public List<GetEventLogResponse> EventLogs { get; set; } = default!;
}
