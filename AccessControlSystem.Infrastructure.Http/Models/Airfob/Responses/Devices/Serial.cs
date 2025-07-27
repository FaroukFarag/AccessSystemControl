using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class Serial
{
    public string Id { get; set; } = default!;
    public string Password { get; set; } = default!;

    [JsonPropertyName("device_id")]
    public long DeviceId { get; set; }
}
