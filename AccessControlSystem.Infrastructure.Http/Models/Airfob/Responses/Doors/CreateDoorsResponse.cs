using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;

public class CreateDoorsResponse
{
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public long SiteId { get; set; }

    public object Status { get; set; } = default!;
    public object Settings { get; set; } = default!;

    [JsonPropertyName("exit_btn_schedule_id")]
    public int ExitBtnScheduleId { get; set; }

    public long Id { get; set; }
}
