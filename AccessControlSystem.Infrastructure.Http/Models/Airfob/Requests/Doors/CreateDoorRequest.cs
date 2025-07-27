using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Doors;

public class CreateDoorRequest
{
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public long SiteId { get; set; }

    public object Settings { get; set; } = default!;
    public object Status { get; set; } = default!;
}
