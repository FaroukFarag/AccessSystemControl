using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;

public class CreateDeviceRequest
{
    public string Serial { get; set; } = default!;
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public long SiteId { get; set; }

    public string Model { get; set; } = default!;

    [JsonPropertyName("model_type")]
    public string ModelType { get; set; } = default!;

    public string Timezone { get; set; } = default!;

    [JsonPropertyName("not_create_door")]
    public bool NotCreateDoor { get; set; } = default!;

    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; } = default!;
}
