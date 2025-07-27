using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;

public class GetDoorResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public long SiteId { get; set; }

    public object Status { get; set; } = default!;

    public object Settings { get; set; } = default!;

    [JsonPropertyName("exit_btn_schedule_id")]
    public int ExitBtnScheduleId { get; set; }

    [JsonPropertyName("in_readers")]
    public IEnumerable<Device> InReaders { get; set; } = default!;

    [JsonPropertyName("out_readers")]
    public IEnumerable<Device> OutReaders { get; set; } = default!;

    [JsonPropertyName("group_names")]
    public IEnumerable<string> GroupNames { get; set; } = default!;
}
