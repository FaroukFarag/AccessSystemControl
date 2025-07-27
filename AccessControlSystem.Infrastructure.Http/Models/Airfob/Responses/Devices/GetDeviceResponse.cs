using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class GetDeviceResponse
{
    public long Id { get; set; }
    public string Serial { get; set; } = default!;
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public long SiteId { get; set; }
    public string Model { get; set; } = default!;

    [JsonPropertyName("model_type")]
    public string ModelType { get; set; } = default!;

    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; } = default!;

    [JsonPropertyName("fw_version")]
    public string FirmwareVersion { get; set; } = default!;

    [JsonPropertyName("keypad_one_time_pin")]
    public string KeypadOneTimePin { get; set; } = default!;

    public Settings Settings { get; set; } = default!;
    public Status Status { get; set; } = default!;
    public string Timezone { get; set; } = default!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("product_name")]
    public string ProductName { get; set; } = default!;

    [JsonPropertyName("door_ids")]
    public IEnumerable<int> DoorIds { get; set; } = default!;

    [JsonPropertyName("group_names")]
    public IEnumerable<string> GroupNames { get; set; } = default!;

    public IEnumerable<object> Modules { get; set; } = default!;

    [JsonPropertyName("elevator_ids")]
    public IEnumerable<object> ElevatorIds { get; set; } = default!;
}
